import cv2
from cvzone.HandTrackingModule import HandDetector
import numpy as np
import time
import random
import warnings
import math
from PyQt5.QtWidgets import QApplication, QDialog, QLabel, QVBoxLayout
from PyQt5.QtCore import QTimer, Qt
from PyQt5.QtGui import QFont
import sys

warnings.filterwarnings("ignore")

# Initialize camera and set resolution to 1920x1080px
cap = cv2.VideoCapture(0)
cap.set(3, 1920)  # Širina prozora
cap.set(4, 1080)  # Visina prozora

# Initialize hand detector
detector = HandDetector(detectionCon=0.8)

# Load images for Level 1
glass_bin = cv2.imread("Slike/Glass_bin.png", cv2.IMREAD_UNCHANGED)
metal_bin = cv2.imread("Slike/Metal_bin.png", cv2.IMREAD_UNCHANGED)
glass_bottle = cv2.imread("Slike/Glass.jpg", cv2.IMREAD_UNCHANGED)

# Load images for Level 2
glass_bin1 = cv2.imread("Slike/Glass_bin.png", cv2.IMREAD_UNCHANGED)
plastic_bin = cv2.imread("Slike/Plastic_bin.jpg", cv2.IMREAD_UNCHANGED)
metal_waste = cv2.imread("Slike/Metal.jpg", cv2.IMREAD_UNCHANGED)

# Load images for Level 3
polluted = cv2.imread("Slike/Polluted_city.jpg", cv2.IMREAD_UNCHANGED)
clean = cv2.imread("Slike/Clean_city.jpg", cv2.IMREAD_UNCHANGED)
hero = cv2.imread("Slike/Hero.jpg", cv2.IMREAD_UNCHANGED)

# Resize images (all bins and bottle have the same size)
bin_size = (220, 220)  # Size for bins
bottle_size = (140, 140)  # Size for waste items
hero_size = (150, 150)  # Size for hero
polluted_size = (250, 200)  # Size for polluted area
glass_bin = cv2.resize(glass_bin, bin_size)
metal_bin = cv2.resize(metal_bin, bin_size)
glass_bottle = cv2.resize(glass_bottle, bottle_size)
glass_bin1 = cv2.resize(glass_bin1, bin_size)
plastic_bin = cv2.resize(plastic_bin, bin_size)
metal_waste = cv2.resize(metal_waste, bottle_size)
polluted = cv2.resize(polluted, polluted_size)
clean = cv2.resize(clean, polluted_size)
hero = cv2.resize(hero, hero_size)

# Positions for bins and waste items
bin1_pos = [200, 60]  # Glass bin position (Level 1)
bin2_pos = [1000, 60]  # Metal bin position (Level 1)
bottle_pos = [600, 530]  # Initial bottle position (Level 1), raised by 20px on y-axis

bin3_pos = [150, 60]  # Glass bin position (Level 2)
bin4_pos = [600, 60]  # Metal bin position (Level 2)
bin5_pos = [1040, 60]  # Plastic bin position (Level 2)
waste_pos = [640, 530]  # Initial waste position (Level 2), raised by 20px on y-axis

clean_pos = [900, 50]  # Clean area position (Level 3)
polluted_pos = [300, 50]  # Polluted area position (Level 3)
hero_pos = [640, 550]  # Initial hero position (Level 3)

# Timer settings
total_duration = 10  # 10 seconds
start_time = time.time()

# Flag to check if the waste is being dragged
is_dragging = False

# Game state
current_level = 1
score = 0

# Function to overlay an image with transparency
def overlay_image(background, overlay, x, y, threshold=240):
    """
    Overlay an image with transparency on top of another image.
    Removes white background from the overlay image.
    """
    h, w = overlay.shape[:2]
    if x < 0 or y < 0 or x + w > background.shape[1] or y + h > background.shape[0]:
        return background  # Skip if out of bounds

    # Ensure overlay has an alpha channel
    if overlay.shape[2] == 3:
        overlay = cv2.cvtColor(overlay, cv2.COLOR_RGB2RGBA)

    # Create a mask for non-white pixels
    gray = cv2.cvtColor(overlay[:, :, :3], cv2.COLOR_RGBA2GRAY)
    mask = gray < threshold  # Pixels darker than threshold are kept

    # Normalize alpha channel based on mask
    alpha = np.where(mask, overlay[:, :, 3] / 255.0, 0)
    inv_alpha = 1.0 - alpha

    # Blend images
    for c in range(3):
        background[y:y + h, x:x + w, c] = (
                alpha * overlay[:, :, c] + inv_alpha * background[y:y + h, x:x + w, c]
        )

    return background

# Function to check if two rectangles are overlapping
def are_rectangles_overlapping(rect1, rect2):
    x1, y1, w1, h1 = rect1
    x2, y2, w2, h2 = rect2
    return (x1 < x2 + w2 and x1 + w1 > x2 and
            y1 < y2 + h2 and y1 + h1 > y2)

# Function to restart the game for the next level
def restart_game():
    global bottle_pos, waste_pos, hero_pos, start_time, is_dragging
    if current_level == 1:
        bottle_pos = [640, 530]  # Reset bottle position for Level 1, raised by 20px
    elif current_level == 2:
        waste_pos = [640, 530]  # Reset waste position for Level 2, raised by 20px
    else:
        hero_pos = [640, 500]  # Reset hero position for Level 3
    start_time = time.time()  # Reset timer
    is_dragging = False  # Reset dragging state

# Custom QDialog for displaying messages
class CustomDialog(QDialog):
    def __init__(self, title, message, color, delay):
        super().__init__()
        self.setWindowTitle(title)
        self.setStyleSheet(f"background-color: {color};")
        self.setWindowFlags(Qt.FramelessWindowHint)  # Remove title bar
        self.setModal(True)

        # Set up layout
        layout = QVBoxLayout()
        self.label = QLabel(message)
        self.label.setAlignment(Qt.AlignCenter)
        self.label.setStyleSheet("font-size: 20px; color: white;")
        layout.addWidget(self.label)
        self.setLayout(layout)

        # Automatically close after delay
        QTimer.singleShot(delay, self.close)

# Function to display a message on the screen using PyQt5
def display_message_pyqt5(title, message, color, delay):
    app = QApplication(sys.argv)
    dialog = CustomDialog(title, message, color, delay)
    dialog.exec_()

# Postavite prozor u full screen mod
cv2.namedWindow('Recycle Game', cv2.WND_PROP_FULLSCREEN)
cv2.setWindowProperty('Recycle Game', cv2.WND_PROP_FULLSCREEN, cv2.WINDOW_FULLSCREEN)

# Main game loop
while True:
    success, img = cap.read()
    if not success:
        break

    # Flip image horizontally for mirror effect
    img = cv2.flip(img, 1)

    # Calculate elapsed time
    elapsed_time = time.time() - start_time
    remaining_time = total_duration - elapsed_time

    # Display timer on the screen (with one decimal place)
    remaining_time_formatted = max(0, round(remaining_time, 1))
    if remaining_time <= 0:
        remaining_time_formatted = 0.0  # Ensure it shows 0.0s when time is up
    cv2.putText(img, f'Vrijeme: {remaining_time_formatted:.1f}s',
                (50, 50), cv2.FONT_HERSHEY_SIMPLEX,
                1, (0, 255, 255), 3, cv2.LINE_AA)

    # Detect hand
    hands, img = detector.findHands(img, flipType=False)
    if hands:
        for hand in hands:
            lmList = hand['lmList']  # List of 21 key points of the hand

            # Get coordinates of the index finger tip (point 8)
            cursor = lmList[8][:2]  # Cursor corresponds to the index finger tip

            # Check if the cursor is near the waste item or hero
            if current_level == 1:
                dist_to_waste = np.linalg.norm(np.array(cursor) - np.array(bottle_pos))
            elif current_level == 2:
                dist_to_waste = np.linalg.norm(np.array(cursor) - np.array(waste_pos))
            else:
                dist_to_waste = np.linalg.norm(np.array(cursor) - np.array(hero_pos))

            # Start dragging if cursor is close to the object
            if dist_to_waste <= 50:  # 50 pixels threshold for dragging
                is_dragging = True

            # If dragging, move the waste item or hero to the cursor position (centered)
            if is_dragging:
                if current_level == 1:
                    bottle_pos = [cursor[0] - bottle_size[0] // 2, cursor[1] - bottle_size[1] // 2]
                elif current_level == 2:
                    waste_pos = [cursor[0] - bottle_size[0] // 2, cursor[1] - bottle_size[1] // 2]
                else:
                    hero_pos = [cursor[0] - hero_size[0] // 2, cursor[1] - hero_size[1] // 2]

    # Draw bins, waste items, or hero based on the current level
    if current_level == 1:
        # Overlay bins for Level 1
        img = overlay_image(img, glass_bin, bin1_pos[0], bin1_pos[1])
        img = overlay_image(img, metal_bin, bin2_pos[0], bin2_pos[1])

        # Overlay waste item for Level 1
        img = overlay_image(img, glass_bottle, bottle_pos[0], bottle_pos[1])

        # Check if the waste item touches the correct bin (Level 1)
        bottle_rect = (bottle_pos[0], bottle_pos[1], bottle_size[0] // 3, bottle_size[1] // 3)  # Smaller hitbox
        bin1_rect = (bin1_pos[0], bin1_pos[1], bin_size[0] // 1.7, bin_size[1] // 1.7)
        bin2_rect = (bin2_pos[0], bin2_pos[1], bin_size[0], bin_size[1])

        if are_rectangles_overlapping(bottle_rect, bin1_rect):
            print("Level 1 = uspješan")
            score += 1
            display_message_pyqt5("Tačan odgovor", "Bravo, uspješno si riješio level, slijedi novi!", "blue", 3000)
            current_level = 2  # Move to Level 2
            restart_game()
        elif are_rectangles_overlapping(bottle_rect, bin2_rect):
            print("Level 1 = Neuspješan")
            score -= 1
            display_message_pyqt5("Pogrešan odgovor", "Staklena boca mora ići u kontenjer namjenjen za staklo!", "red", 5000)
            current_level = 2  # Move to Level 2
            restart_game()
    elif current_level == 2:
        # Overlay bins for Level 2
        img = overlay_image(img, glass_bin1, bin3_pos[0], bin3_pos[1])
        img = overlay_image(img, metal_bin, bin4_pos[0], bin4_pos[1])
        img = overlay_image(img, plastic_bin, bin5_pos[0], bin5_pos[1])

        # Overlay waste item for Level 2
        img = overlay_image(img, metal_waste, waste_pos[0], waste_pos[1])

        # Check if the waste item touches the correct bin (Level 2)
        waste_rect = (waste_pos[0], waste_pos[1], bottle_size[0] // 2, bottle_size[1] // 2)  # Smaller hitbox
        bin3_rect = (bin3_pos[0], bin3_pos[1], bin_size[0], bin_size[1] // 2)
        bin4_rect = (bin4_pos[0], bin4_pos[1], bin_size[0], bin_size[1] // 2)
        bin5_rect = (bin5_pos[0], bin5_pos[1], bin_size[0], bin_size[1] // 2)

        if are_rectangles_overlapping(waste_rect, bin4_rect):
            print("Level 2 = uspješan")
            score += 1
            display_message_pyqt5("Tačan odgovor", "Bravo, uspješno si riješio level, slijedi novi!", "blue", 3000)
            current_level = 3  # Move to Level 3
            restart_game()
        elif are_rectangles_overlapping(waste_rect, bin3_rect) or \
                are_rectangles_overlapping(waste_rect, bin5_rect):
            print("Level 2 = Neuspješan")
            score -= 1
            display_message_pyqt5("Pogrešan odgovor", "Metalni otpad mora ići u kontenjer namjenjen za metal!", "red", 5000)
            current_level = 3  # Move to Level 3
            restart_game()
    else:
        # Overlay polluted area, clean area, and hero for Level 3
        img = overlay_image(img, polluted, polluted_pos[0], polluted_pos[1])
        img = overlay_image(img, clean, clean_pos[0], clean_pos[1])

        # Ensure hero_pos is within image bounds
        hero_pos[0] = max(0, min(hero_pos[0], img.shape[1] - hero_size[0]))
        hero_pos[1] = max(0, min(hero_pos[1], img.shape[0] - hero_size[1]))

        # Overlay hero for Level 3
        img = overlay_image(img, hero, hero_pos[0], hero_pos[1])

        # Check if the hero touches the polluted area or clean area (Level 3)
        hero_rect = (hero_pos[0], hero_pos[1], hero_size[0] // 2, hero_size[1] // 2)  # Smaller hitbox
        polluted_rect = (polluted_pos[0], polluted_pos[1], polluted_size[0], polluted_size[1] // 1.7)
        clean_rect = (clean_pos[0], clean_pos[1], polluted_size[0], polluted_size[1] // 1.7)

        if are_rectangles_overlapping(hero_rect, polluted_rect):
            print("Level 3 = uspješan")
            score += 1
            display_message_pyqt5("Tačan odgovor", "Bravo, uspješno si riješio level. Kraj testa!", "blue", 3000)
            break  # End the game
        elif are_rectangles_overlapping(hero_rect, clean_rect):
            print("Level 3 = Neuspješan")
            score -= 1
            display_message_pyqt5("Pogrešan odgovor", "Pogrešan odgovor\nHeroj mora poći u zagadjen grad!", "red", 5000)
            break  # End the game
        elif remaining_time <= 0:
            print("Level 3 = Neuspješan (Vrijeme isteklo)")
            score -= 1
            display_message_pyqt5("Pogrešan odgovor", "Heroj mora poći u zagadjen grad!", "red", 5000)
            break  # End the game

    # After timer expires
    if remaining_time <= 0:
        print(f"Vrijme je isteklo! Level {current_level} = Neuspješan")
        score -= 1
        if current_level == 1:
            display_message_pyqt5("Isteklo vrijeme", "Vrijeme je isteklo!\nStaklena boca mora ići u kontenjer namjenjen za staklo!", "red", 5000)
            current_level = 2  # Move to Level 2
            restart_game()
        elif current_level == 2:
            display_message_pyqt5("Isteklo vrijeme", "Vrijeme je isteklo!\nMetalni otpad mora ići u kontenjer namjenjen za metal!", "red", 5000)
            current_level = 3  # Move to Level 3
            restart_game()
        else:
            display_message_pyqt5("Isteklo vrijeme",
                                  "Vrijeme je isteklo!\nHeroj mora poći u zagadjen grad!", "red",
                                  5000)
            break  # End the game

    # Display image with messages and shapes in the window
    cv2.imshow('Recycle Game', img)
    # Handle key events
    key = cv2.waitKey(1)
    if key & 0xFF == ord('q'):
        break

# Release resources
cap.release()
cv2.destroyAllWindows()

# Display final score
print(f"Konačni rezultat: {score}")