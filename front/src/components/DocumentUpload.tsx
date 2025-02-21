import React, { useState } from 'react';
import agent from '../data/agent';

const DocumentUpload = () => {
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [message, setMessage] = useState<string | null>(null);
  const token = 'your_token'; // Zameni sa stvarnim tokenom

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      setFile(event.target.files[0]);
      setError(null);
    }
  };

  const handleUpload = async () => {
    if (!file) {
      setError('Molimo odaberite fajl.');
      return;
    }

    try {
      const response = await agent.Documents.upload(file, token);
      setMessage('Dokument uspešno uploadovan!');

      // Prikazuj JMB u konzoli
      console.log('Prepoznati JMB:', response.jmb);
    } catch (err) {
      setError('Došlo je do greške prilikom uploadovanja dokumenta.');
      console.error(err);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center p-6 bg-gray-100 rounded-lg shadow-md">
      <h2 className="text-xl font-bold mb-4">Uploaduj Dokument</h2>
      <input
        type="file"
        onChange={handleFileChange}
        className="mb-4 p-2 border border-gray-300 rounded-md w-full max-w-xs"
      />
      <button
        onClick={handleUpload}
        className="bg-blue-500 text-white font-semibold py-2 px-4 rounded-md hover:bg-blue-600 transition duration-200"
      >
        Uploaduj dokument
      </button>
    </div>
  );
};

export default DocumentUpload;
