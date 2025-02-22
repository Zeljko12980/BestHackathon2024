import React from 'react';
import { useParams, NavLink } from 'react-router-dom';
import axios from 'axios';

const ClassDetails: React.FC = () => {
  const { classId } = useParams<{ classId: string }>();

  const handlePlayGame = async () => {
    try {
      // Pozivanje metode sa backend-a (FaceRecognitionController)
      const response = await axios.post(
        'http://localhost:5024/api/facerecognition/prepoznaj-lice',
      );
      const data = response.data;

      // Ako je prepoznavanje uspelo, možeš da uradiš nešto sa `data`
      console.log(data);
    } catch (error) {
      // Obrađivanje greške ako se desi
      console.error('Greška pri pozivu API-ja:', error);
      alert('Došlo je do greške prilikom prepoznavanja.');
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto bg-white shadow-lg rounded-lg">
      <h1 className="text-3xl font-semibold text-center text-gray-800 mb-6">
        Detalji časa u razredu: {classId}
      </h1>

      {/* Prvi div - Eko igra sa dugmetom */}
      <div className="mb-8 p-6 bg-green-100 rounded-lg shadow-md">
        <h2 className="text-2xl font-semibold text-green-800 mb-4">Eko igra</h2>
        <button
          onClick={handlePlayGame}
          className="px-6 py-3 bg-green-500 text-white font-semibold rounded-full hover:bg-green-600 transition duration-300"
        >
          Počni igru
        </button>
      </div>

      {/* Drugi div - Statistika sa linkom */}
      <div className="p-6 bg-blue-100 rounded-lg shadow-md">
        <h2 className="text-2xl font-semibold text-blue-800 mb-4">
          Statistika
        </h2>
        <NavLink
          to={`/statistika/${classId}`}
          className="text-blue-500 hover:text-blue-700 font-semibold text-lg"
        >
          Vidi statistiku
        </NavLink>
      </div>
    </div>
  );
};

export default ClassDetails;
