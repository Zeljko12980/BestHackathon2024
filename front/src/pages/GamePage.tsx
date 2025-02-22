import React, { useState } from 'react';
import axios from 'axios';
import { useLocation } from 'react-router-dom';
import { FaRegPlayCircle, FaTrophy, FaFireAlt } from 'react-icons/fa';

const GamePage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const location = useLocation();
  const data = location.state?.data;
  const userId = data.id;

  const handleStartGameOne = async () => {
    setIsLoading(true);
    setError(null); // Resetovanje greške pre novog poziva
    try {
      const response = await axios.post(
        `http://localhost:5024/api/game/start-game?userId=${userId}`,
      );

      if (response.data.message === 'Game started successfully') {
        alert('Igra je uspešno pokrenuta!');
      }
    } catch (error) {
      setError('Greška pri pokretanju igre');
      console.error('Greška pri pozivu API-ja:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleStartGameTwo = async () => {
    setIsLoading(true);
    setError(null); // Resetovanje greške pre novog poziva
    try {
      const response = await axios.get(
        `http://localhost:5024/api/Python/run-quiz?userId=${userId}`,
      );

      if (response.data.message === 'Game started successfully') {
        alert('Igra je uspešno pokrenuta!');
      }
    } catch (error) {
      setError('Greška pri pokretanju igre');
      console.error('Greška pri pozivu API-ja:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 text-gray-900">
      <div className="p-10 bg-white rounded-lg shadow-2xl text-center w-[400px] md:w-[500px] space-y-6">
        <h2 className="text-4xl font-bold mb-4 text-blue-600">📚 EduZone </h2>
        <h4 className="text-lg mb-2 text-gray-700">
          Dobrodošao,{' '}
          <span className="font-semibold text-blue-500">{data?.firstName}</span>
          !
        </h4>

        <p className="text-gray-600 mb-4 text-md">
          Ovdje možete da igrate igre koje će vam pomoći da učite i zabavljate
          se! Izaberite igru koja vam se najviše dopada i počnite da učite na
          zabavan način.
        </p>

        {message && (
          <p className="text-yellow-500 font-medium mb-4">{message}</p>
        )}

        <div className="space-y-4 mt-6">
          <div className="game-button">
            <button
              onClick={handleStartGameOne}
              disabled={loading}
              className={`w-full py-3 text-lg font-semibold rounded-lg transition-all duration-300 shadow-md ${loading ? 'bg-gray-400' : 'bg-blue-500 hover:bg-blue-600 text-white'}`}
            >
              {loading ? (
                '⏳ Pokrećem igru...'
              ) : (
                <>
                  <FaRegPlayCircle className="inline-block mr-2" /> Igra 1 -
                  Učenje kroz igru
                </>
              )}
            </button>
            <p className="text-gray-500 text-sm mt-2">
              Ova igra pomaže da naučiš osnovne pojmove na zabavan način kroz
              različite zadatke i izazove.
            </p>
          </div>

          <div className="game-button">
            <button
              onClick={handleStartGameTwo}
              disabled={loading}
              className="w-full py-3 text-lg font-semibold bg-green-500 rounded-lg shadow-md hover:bg-green-600 text-white transition-all duration-300"
            >
              <FaTrophy className="inline-block mr-2" /> Igra 2 - Izazov u
              učenju
            </button>
            <p className="text-gray-500 text-sm mt-2">
              Igra koja testira tvoje znanje uz izazove koji postaju sve teži.
            </p>
          </div>

          <div className="game-button">
            <button className="w-full py-3 text-lg font-semibold bg-red-500 rounded-lg shadow-md hover:bg-red-600 text-white transition-all duration-300">
              <FaFireAlt className="inline-block mr-2" /> Igra 3 - Brzi testovi
            </button>
            <p className="text-gray-500 text-sm mt-2">
              U ovoj igri moraš brzo odgovarati na pitanja kako bi testirao
              svoje znanje pod pritiskom.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default GamePage;
