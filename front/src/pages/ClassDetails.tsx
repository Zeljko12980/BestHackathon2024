import React, { useState } from 'react';
import { useParams, NavLink, useNavigate } from 'react-router-dom';
import axios from 'axios';

const ClassDetails: React.FC = () => {
  const { classId } = useParams<{ classId: string }>();
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handlePlayGame = async () => {
    try {
      setIsLoading(true);
      const response = await axios.post(
        'http://localhost:5024/api/auth/prepoznaj-lice',
      );
      const data = response.data;
      const studentId = data.id;
      console.log('id studenta: ', studentId);
      navigate('/game', { state: { data } });
    } catch (error) {
      console.error('Greška pri pozivu API-ja:', error);
      alert('Došlo je do greške prilikom prepoznavanja.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto bg-white shadow-xl rounded-lg">
      <h1 className="text-3xl font-semibold text-center text-gray-800 mb-6">
        Razred {classId}
      </h1>

      {/* Eko igra */}
      <div className="mb-8 p-8 bg-green-50 rounded-lg shadow-lg flex flex-col items-center space-y-6">
        <h2 className="text-2xl font-semibold text-green-800 mb-4">Eko igra</h2>

        {/* Dugme za početak igre */}
        <button
          onClick={handlePlayGame}
          disabled={isLoading}
          className={`px-8 py-4 ${isLoading ? 'bg-gray-400' : 'bg-green-500'} text-white font-semibold rounded-full hover:bg-green-600 transition duration-300`}
        >
          {isLoading ? '⏳ Pokrećem igru...' : 'Zadajte kviz'}
        </button>
      </div>

      {/* Statistika */}
      <div className="p-8 bg-blue-50 rounded-lg shadow-lg">
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
