from flask import Flask, jsonify
import face_recognition
import cv2
import os
import numpy as np

app = Flask(__name__)

# Folder sa poznatim licima
poznati_folder = "Poznati"

# Učitavanje i enkodiranje poznatih lica
poznati_encodings = []
poznata_imena = []

for file_name in os.listdir(poznati_folder):
    file_path = os.path.join(poznati_folder, file_name)
    known_image = face_recognition.load_image_file(file_path)
    known_encodings = face_recognition.face_encodings(known_image)

    if known_encodings:
        poznati_encodings.append(known_encodings[0])
        poznata_imena.append(os.path.splitext(file_name)[0])  # Uzimamo ime fajla bez ekstenzije

@app.route('/prepoznaj-lice', methods=['POST'])
def prepoznaj_lice():
    # Pokretanje kamere
    video_capture = cv2.VideoCapture(0)
    prepoznat = False  # Da pratimo da li smo nekog prepoznali

    # Dodajemo timeout (npr. 30 sekundi) za pokušaj prepoznavanja
    timeout = 30  # sekundi
    start_time = cv2.getTickCount()

    try:
        while True:
            # Proveravamo da li je prošlo više od 30 sekundi
            elapsed_time = (cv2.getTickCount() - start_time) / cv2.getTickFrequency()
            if elapsed_time > timeout:
                return jsonify({"message": "Prepoznavanje traje predugo, pokušajte ponovo."}), 408

            # Čitanje kadra sa kamere
            ret, frame = video_capture.read()
            if not ret:
                return jsonify({"message": "Greška pri pristupu kameri"}), 500

            # Pronalazak lica u kadru
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            face_locations = face_recognition.face_locations(rgb_frame)
            face_encodings = face_recognition.face_encodings(rgb_frame, face_locations)

            for face_encoding in face_encodings:
                # Upoređujemo sa svim poznatim licima
                matches = face_recognition.compare_faces(poznati_encodings, face_encoding, tolerance=0.5)
                face_distances = face_recognition.face_distance(poznati_encodings, face_encoding)

                best_match_index = np.argmin(face_distances) if len(face_distances) > 0 else None

                if best_match_index is not None and matches[best_match_index]:
                    ime = poznata_imena[best_match_index]
                    print(f"Welcome, {ime}")
                    prepoznat = True
                    video_capture.release()
                    cv2.destroyAllWindows()
                    return jsonify(ime), 200  # Vraćamo ime prepoznate osobe

            # Ako pritisnemo 'q', prekidamo petlju
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

    finally:
        video_capture.release()
        cv2.destroyAllWindows()

    if not prepoznat:
        return jsonify({"message": "Nepoznata osoba"}), 400  # Ako nije prepoznata osoba

if __name__ == "__main__":
    app.run(debug=True, host='0.0.0.0', port=5000)
