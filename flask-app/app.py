from flask import Flask, jsonify, request
from ai_reco import ucitaj_poznate_lice, prepoznavanje_lica
import cv2
import subprocess
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

@app.route('/run-game', methods=['GET'])
def run_game():
    try:
        user_id = request.args.get('userId')
        subprocess.Popen(['python', 'game_level_one.py', user_id])  # Koristite samo 'python' ako koristite Windows
        print(user_id)

        return jsonify({"message": "Game started successfully"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500
    
@app.route('/run-quizz', methods=['GET'])
def run_quizz():
    try:
        user_id = request.args.get('userId')
        subprocess.Popen(['python', 'quizz.py', user_id])  # Koristite samo 'python' ako koristite Windows
        print(user_id)

        return jsonify({"message": "Quizz started successfully"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500    

@app.route('/run-left', methods=['GET'])
def run_quizz():
    try:
        user_id = request.args.get('userId')
        subprocess.Popen(['python', 'left_right.py', user_id])  # Koristite samo 'python' ako koristite Windows
        print(user_id)

        return jsonify({"message": "Quizz started successfully"}), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500  

@app.route('/prepoznaj-lice', methods=['POST'])
def prepoznaj_lice():
    video_capture = cv2.VideoCapture(0)
    if not video_capture.isOpened():
        return jsonify({"error": "Could not open camera"}), 500

    start_time = cv2.getTickCount()
    prepoznat = False

    try:
        while True:
            ime = prepoznavanje_lica(video_capture)
            if ime:
                
                prepoznat = True
                video_capture.release()
                cv2.destroyAllWindows()
                return jsonify(ime), 200  # Vraćamo ime prepoznate osobe

            # Ako pritisnemo 'q', prekidamo petlju
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

    except Exception as e:
        return jsonify({"message": str(e)}), 500

    finally:
        video_capture.release()
        cv2.destroyAllWindows()

    if not prepoznat:
        return jsonify({"message": "Nepoznata osoba"}), 400  # Ako nije prepoznata osoba

if __name__ == "__main__":
    ucitaj_poznate_lice()
    app.run(debug=True, host='0.0.0.0', port=5000)
