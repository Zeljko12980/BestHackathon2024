import face_recognition
import cv2
import os
import numpy as np

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

# Pokretanje kamere
video_capture = cv2.VideoCapture(0)

print("Čekam lice...")

prepoznat = False  # Da pratimo da li smo nekog prepoznali

while True:
    # Čitanje kadra sa kamere
    ret, frame = video_capture.read()

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
            break  # Prekidamo jer smo prepoznali osobu

    # Prikaz videa
    cv2.imshow('Video', frame)

    # Ako je osoba prepoznata, izlazimo iz petlje
    if prepoznat:
        break

    # Ako pritisnemo 'q', prekidamo petlju
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Zatvaranje kamere
video_capture.release()
cv2.destroyAllWindows()

if not prepoznat:
    print("Nepoznata osoba")
