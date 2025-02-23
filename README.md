# EcoLearn - Edukativna platforma za ekološku svijest

## **Opis projekta**

EcoLearn je interaktivna web platforma dizajnirana za podizanje ekološke svijesti kod djece kroz edukativne igre, kvizove i proširenu stvarnost (AR). Korišćenjem modernih tehnologija, platforma omogućava angažovanje dece u zabavnom i edukativnom okruženju.

---

## **Tehnologije korišćene u projektu**

- **Frontend:** React (TypeScript) + Tailwind CSS  
- **Backend:** .NET Web API  
- **AI modul:** Python + Flask  

### **Biblioteke i alati:**
- Proširena stvarnost (AR)  
- Prepoznavanje lica  
- Analiza pokreta ruku  
- Generisanje govora  

---

## **Instalacija i pokretanje**

### **Frontend**
bash
`cd backend`
`dotnet watch run`

### **Frontend**
bash
`cd frontend`
`npm install`
`npm start`



### **AI modul (Python + Flask)**
`pip install face_recognition opencv-python numpy flask flask-cors mediapipe requests gtts cvzone pillow`
`python app.py`

EcoLearn/
│── frontend/         # React + Tailwind frontend
│── backend/          # .NET Web API backend
│── ai_module/        # Python + Flask AI modul
│── README.md         # Dokumentacija


### Funkcionalnosti
#### Edukativne igre
Interaktivne igre koje uče decu o ekologiji kroz zabavu.

Analiza pokreta ruku pomoću Mediapipe i OpenCV.

#### Kvizovi
Različiti nivoji kvizova sa pitanjima o zaštiti životne sredine.

Implementacija pomoću React-a i backend API-ja.


#### Prepoznavanje lica
Koristi `face_recognition` biblioteku za prepoznavanje lica u realnom vremenu.

Primena u interaktivnim igrama i edukativnim aktivnostima.


 #### Govorna povratna informacija
 Koristi `gTTS` za generisanje glasovnih poruka u igrama i kvizovima.
