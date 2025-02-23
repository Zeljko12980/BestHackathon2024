import csv
import cv2
import cvzone as cvz
from cvzone.HandTrackingModule import HandDetector
import time
from PIL import Image, ImageDraw, ImageFont
import numpy as np
from gtts import gTTS
import os
import sys
import requests

# Inicijalizacija kamere
cap = cv2.VideoCapture(0)
cap.set(3, 1280)
cap.set(4, 720)

detector = HandDetector(detectionCon=0.8)

# Klasa za MCQ pitanja
class MCQ():
    def __init__(self, data):
        self.question = data[0]
        self.choice1 = data[1]
        self.choice2 = data[2]
        self.choice3 = data[3]
        self.choice4 = data[4]
        self.answer = int(data[5])
        self.userAns = None

    def update(self, cursor, bboxs):
        for x, box in enumerate(bboxs):
            x1, y1, x2, y2 = box
            if x1 < cursor[0] < x2 and y1 < cursor[1] < y2:
                self.userAns = x + 1
                return True
        return False

# Učitavanje CSV fajla
getFile = "Pitanja.csv"
with open(getFile, newline='\n', encoding='utf-8') as file:
    reader = csv.reader(file)
    datafile = list(reader)[1:]

mcqList = [MCQ(q) for q in datafile]

quesNumber = 0
qTotal = len(datafile)

# Funkcija za prikazivanje UTF-8 teksta pomoću Pillow-a sa pozadinom
def draw_text_with_bg_pil(frame, text, position, font_size=32, bg_color=(0, 0, 0), text_color=(255, 255, 255),
                          padding=15, border=4):
    pil_image = Image.fromarray(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
    draw = ImageDraw.Draw(pil_image)
    font = ImageFont.truetype("arial.ttf", font_size)

    # Izračunaj veličinu teksta
    text_bbox = draw.textbbox((0, 0), text, font=font)
    text_width = text_bbox[2] - text_bbox[0]
    text_height = text_bbox[3] - text_bbox[1]

    # Koordinate pozadine
    x, y = position
    bg_x1 = x - padding
    bg_y1 = y - padding
    bg_x2 = x + text_width + padding
    bg_y2 = y + text_height + padding

    # Nacrtaj pozadinu i border
    draw.rectangle([bg_x1, bg_y1, bg_x2, bg_y2], fill=bg_color, outline="white", width=border)

    # Nacrtaj tekst
    draw.text((x, y), text, font=font, fill=text_color)

    return cv2.cvtColor(np.array(pil_image), cv2.COLOR_RGB2BGR), (bg_x1, bg_y1, bg_x2, bg_y2)

# Funkcija za prelamanje teksta na dve linije
def wrap_text(text, max_length):
    if len(text) > max_length:
        mid = text.rfind(' ', 0, max_length)
        if mid == -1:
            mid = max_length
        return text[:mid], text[mid:]
    return text, ""

# Funkcija za reprodukciju zvuka
def play_sound(text, filename="output.mp3"):
    tts = gTTS(text, lang='sr')  # Postavite jezik na srpski
    tts.save(filename)
    os.system(f"afplay {filename}")  # Za macOS
    # Za Windows koristite: os.system(f"start {filename}")
    # Za Linux koristite: os.system(f"mpg321 {filename}")

# Fullscreen prozor
cv2.namedWindow("Frame", cv2.WND_PROP_FULLSCREEN)
cv2.setWindowProperty("Frame", cv2.WND_PROP_FULLSCREEN, cv2.WINDOW_FULLSCREEN)

# Timer varijable
start_time = time.time()
time_limit = 15  # 15 sekundi po pitanju

#Tekstovi za netačne odgovore (ispravljeni redoslijed i prilagođeni za djecu)
incorrect_texts = [
    "Netačan odgovor. Industrija sagoreva fosilna goriva i stvara mnogo ugljen-dioksida, što zagreva našu planetu.",
    "Netačan odgovor. Sunce i vetar prave struju bez zagađivanja, a to je bolje za našu planetu.",
    "Netačan odgovor. Ugljen-dioksid zagreva Zemlju, a previše ga stvara globalno zagrevanje.",
    "Netačan odgovor. Vozila koja koriste benzin ili dizel zagađuju vazduh i zagrevaju planetu.",
    "Netačan odgovor. U Americi svaka osoba stvara puno ugljen-dioksida, što zagađuje vazduh više nego u drugim zemljama."
]
while True:
    ret, frame = cap.read()
    frame = cv2.flip(frame, 1)

    hands, frame = detector.findHands(frame, flipType=False)

    if quesNumber < qTotal:
        mcq = mcqList[quesNumber]

        # Ukupno pitanja
        frame = draw_text_with_bg_pil(frame, f'Ukupno pitanja: {qTotal}', (50, 50), 40, (0, 0, 0))[0]

        # Prikaz pitanja (može se prelomiti u dve linije)
        question_line1, question_line2 = wrap_text(mcq.question, 50)
        frame = draw_text_with_bg_pil(frame, question_line1, (150, 150), 38, (0, 0, 0))[0]
        if question_line2:
            frame = draw_text_with_bg_pil(frame, question_line2, (150, 200), 38, (0, 0, 0))[0]

        # Prikaz odgovora sa custom funkcijom
        frame, box1 = draw_text_with_bg_pil(frame, mcq.choice1, (150, 300), 32, (50, 50, 50))
        frame, box2 = draw_text_with_bg_pil(frame, mcq.choice2, (600, 300), 32, (50, 50, 50))
        frame, box3 = draw_text_with_bg_pil(frame, mcq.choice3, (150, 450), 32, (50, 50, 50))
        frame, box4 = draw_text_with_bg_pil(frame, mcq.choice4, (600, 450), 32, (50, 50, 50))

        # Detekcija ruke
        if hands:
            lmList = hands[0]['lmList']
            cursor = lmList[8][:2]  # Vrh kažiprsta
            cursor2 = lmList[12][:2]  # Vrh srednjeg prsta

            result = detector.findDistance(cursor, cursor2)
            length = result[0]

            # Provera kliknutog odgovora
            if 20 <= length <= 30:
                if mcq.update(cursor, [box1, box2, box3, box4]):
                    if mcq.userAns == mcq.answer:
                        # Tačan odgovor
                        correct_box = [box1, box2, box3, box4][mcq.answer - 1]
                        frame = draw_text_with_bg_pil(frame,
                                                      [mcq.choice1, mcq.choice2, mcq.choice3, mcq.choice4][
                                                          mcq.answer - 1],
                                                      (correct_box[0] + 15, correct_box[1] + 15), 32, (0, 200, 0),
                                                      (255, 255, 255)
                                                      )[0]
                        cv2.imshow("Frame", frame)
                        cv2.waitKey(100)  # Kratka pauza za prikaz zelenog odgovora
                        play_sound("Tačan odgovor.")  # Reprodukcija zvuka za tačan odgovor
                        quesNumber += 1
                        start_time = time.time()
                    else:
                        # Netačan odgovor
                        incorrect_box = [box1, box2, box3, box4][mcq.userAns - 1]
                        correct_box = [box1, box2, box3, box4][mcq.answer - 1]

                        # Oboji netačno crveno
                        frame = draw_text_with_bg_pil(frame,
                                                      [mcq.choice1, mcq.choice2, mcq.choice3, mcq.choice4][
                                                          mcq.userAns - 1],
                                                      (incorrect_box[0] + 15, incorrect_box[1] + 15), 32, (200, 0, 0),
                                                      (255, 255, 255)
                                                      )[0]

                        # Oboji tačno zeleno
                        frame = draw_text_with_bg_pil(frame,
                                                      [mcq.choice1, mcq.choice2, mcq.choice3, mcq.choice4][
                                                          mcq.answer - 1],
                                                      (correct_box[0] + 15, correct_box[1] + 15), 32, (0, 200, 0),
                                                      (255, 255, 255)
                                                      )[0]

                        cv2.imshow("Frame", frame)
                        cv2.waitKey(100)  # Kratka pauza za prikaz crvenog i zelenog odgovora
                        play_sound(incorrect_texts[quesNumber])  # Pusti odgovarajući zvuk
                        quesNumber += 1
                        start_time = time.time()

        # Tajmer
        elapsed_time = time.time() - start_time
        remaining_time = max(0, time_limit - int(elapsed_time))
        frame = draw_text_with_bg_pil(frame, f'Vrijeme: {remaining_time}s', (1000, 50), 40, (0, 0, 0))[0]

        # Ako vreme istekne
        if elapsed_time >= time_limit:
            # Prikaži tačan odgovor u zelenoj boji
            correct_box = [box1, box2, box3, box4][mcq.answer - 1]
            frame = draw_text_with_bg_pil(frame,
                                          [mcq.choice1, mcq.choice2, mcq.choice3, mcq.choice4][mcq.answer - 1],
                                          (correct_box[0] + 15, correct_box[1] + 15), 32, (0, 200, 0),
                                          (255, 255, 255)
                                          )[0]
            cv2.imshow("Frame", frame)
            cv2.waitKey(100)  # Kratka pauza za prikaz zelenog odgovora
            play_sound(incorrect_texts[quesNumber])  # Pusti zvuk za isteklo vrijeme
            quesNumber += 1
            start_time = time.time()

    else:
        # Kraj kviza
        score = sum(1 for mcq in mcqList if mcq.answer == mcq.userAns)
        print(f"{score}")  # Ispis u konzoli
        api_url = f'http://localhost:5024/api/Score/increase-score/{sys.argv[1]}/{score}'

        # Pozivanje API-ja sa PUT metodom
        response = requests.put(api_url)

# Provera odgovora od API-ja
        if response.status_code == 200:
         print(f"Uspešno upisan rezultat za korisnika {sys.argv[1]}")
        else:
         print(f"Došlo je do greške pri upisu rezultata. Status kod: {response.status_code}")

        for file in os.listdir():
            if file.endswith(".mp3"):  # Provera da li fajl ima ekstenziju .mp3
                os.remove(file)  # Brisanje fajla

        # Prikaz "Kraj kviza" na ekranu
        frame = draw_text_with_bg_pil(frame, "Kraj kviza!", (450, 300), 50, (0, 0, 0), (0, 255, 0))[0]
        cv2.imshow("Frame", frame)
        cv2.waitKey(1500)  # Pauza 1.5 sekunde
        break

    # Progress bar
    Probar = 150 + (900 // qTotal) * quesNumber
    cv2.rectangle(frame, (150, 600), (Probar, 620), (17, 171, 50), cv2.FILLED)
    cv2.rectangle(frame, (150, 600), (1050, 620), (255, 255, 255), 5)
    frame = draw_text_with_bg_pil(frame, f'{round((quesNumber / qTotal) * 100)}%', (1130, 635), 32, (0, 0, 0))[0]

    cv2.imshow("Frame", frame)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        for file in os.listdir():
            if file.endswith(".mp3"):  # Provera da li fajl ima ekstenziju .mp3
                os.remove(file)  # Brisanje fajla
        break

cv2.destroyAllWindows()