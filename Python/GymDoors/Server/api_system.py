from flask import Flask, jsonify
from gym_system import GymSystem
import datetime

app = Flask(__name__)
gym_manager = GymSystem()

@app.route('/enter/<input>')
def enter(input):
    if ';' in input and len(input.split(';')) == 2: 
        response = gym_manager.enter(input)
        print(f"Intento de entrada con DNI: {input.split(';')[0]} a las {datetime.datetime.now()}, respuesta: {response['message']}")
        return jsonify(response)
    else:
        return 'Error con el formato del input'
    


@app.route('/exit/<input>')
def exit(input):
    if ';' in input and len(input.split(';')) == 2: 
        response = gym_manager.exit(input)
        print(f"Intento de salida con DNI: {input.split(';')[0]} a las {datetime.datetime.now()}, respuesta: {response['message']}")
        return jsonify(response)
    else:
        return 'Error con el formato del input'

if __name__ == '__main__':
    app.run(debug=True)