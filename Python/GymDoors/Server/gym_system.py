import os, json, firebase_admin, cv2, segno, uuid, datetime
from pyzbar.pyzbar import decode
from PIL import Image
from firebase_admin import credentials, initialize_app, firestore

CONST_NAME_FILE = "firebase_config.json"

CONST_COLLECTION_GYM_NAME = "Gym"

class GymSystem:

    def __init__(self):
        current_path = os.path.dirname(os.path.abspath(__name__))

        firebase_config = None

        with open(current_path + "\\" + CONST_NAME_FILE) as json_file:
            firebase_config = json.load(json_file)

        cred = credentials.Certificate(firebase_config)

        firebase_admin.initialize_app(cred)

        self._db = firestore.client()

        self._db_collection = self._db.collection(CONST_COLLECTION_GYM_NAME)

    def add_client(self, data):
        data = json.loads(data)
        if(self._db_collection.document(data['dni']).get().exists is False):
            data['enter_code'] = str(uuid.uuid4())
            print('Añadiendo cliente...')
            return self._db_collection.document(data['dni']).set(data)
        else:
            print('Ya existe un cliente con el mismo DNI')

    def remove_client(self, DNI):
        doc_ref = self._db_collection.document(DNI)

        if(doc_ref.get().exists):
            return(doc_ref.delete())
        

    def add_payment(self, DNI, paid, payment_method):
        doc_ref = self._db_collection.document(DNI)
        if(doc_ref.get().exists):

            today = datetime.datetime.now() 

            data = {
                'client':doc_ref,
                'is_paid':paid,
                'payment_method':payment_method,
                'date':today
            }

            self._db_collection.document(f"{DNI};{today.month};{today.year}").set(data)
        else:
            print("El usuario no existe")

    def enter(self, input):
        DNI = input.split(';')[0]
        enter_code = input.split(';')[1]
        doc_ref = self._db_collection.document(DNI)
        doc = doc_ref.get()
        if(doc.exists):
            data = doc.to_dict()
            if(data['subscribed'] == True):
                if(data['enter_code'] == enter_code and data['dni'] == DNI):
                    today = datetime.datetime.now()

                    if(self._db_collection.document(DNI + ";" + str(today.month) + ";" + str(today.year)).get().to_dict()['is_paid'] == True):
                        if(data['entered'] == True):
                            return {
                                'success':False,
                                'message':'Error: Ya ha entrado alguien con su código'
                            }
                        else:
                            doc_ref.update({'entered':True})
                            return {
                                'success':True,
                                'message':'Entrada abierta'
                            }
                    else:
                        return {
                            'success':False,
                            'message':'Error: No se ha efectuado correctamente el ultimo pago'
                        }
                else:
                    return {
                        'success':False,
                        'message':'Error: Los datos de entrada no coinciden'
                    }
            else:
                return {
                    'success':False,
                    'message':'Error: El usuario no está suscrito'
                }
        else:
            return {
                'success':False,
                'message':'Error: El documento no existe'
            }

    def exit(self, input):
        DNI = input.split(';')[0]
        enter_code = input.split(';')[1]
        doc_ref = self._db_collection.document(DNI)
        doc = doc_ref.get()
        if(doc.exists):
            data = doc.to_dict()
            if(data['subscribed'] == True):
                if(data['enter_code'] == enter_code and data['dni'] == DNI):
                    today = datetime.datetime.now()

                    if(self._db_collection.document(DNI + ";" + str(today.month) + ";" + str(today.year)).get().to_dict()['is_paid'] == True):
                        if(data['entered'] == True):
                            doc_ref.update({'entered':False})
                            return {
                                'success':True,
                                'message':'Salida abierta'
                            }
                        else:
                            return {
                                'success':False,
                                'message':'No figura que esté dentro del gimnasio'
                            }
                    else:
                        return {
                            'success':False,
                            'message':'Error: No se ha efectuado correctamente el ultimo pago'
                        }
                else:
                    return {
                        'success':False,
                        'message':'Error: Los datos de entrada no coinciden'
                    }
            else:
                return {
                    'success':False,
                    'message':'Error: El usuario no está suscrito'
                }
        else:
            return {
                'success':False,
                'message':'Error: El documento no existe'
            }

    def get_qr_code(self, DNI):
        doc = self._db_collection.document(DNI).get()
        today = datetime.datetime.now()
        if(doc.exists):
            return segno.make_qr(f"{doc.to_dict()['dni']};{doc.to_dict()['enter_code']}").save("enter_permission_qr.png", scale=10)
        else:
            print("The document doesn't exist")
            return None
    
