import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import agent from '../data/agent';

// Mapa koja konvertuje className u classId
const classMapping: Record<string, number> = {
  I1: 1,
  I2: 2,
  I3: 3,
  IV1: 4,
  IV2: 5,
  IV3: 6,
};

const Statistika: React.FC = () => {
  const { className } = useParams<{ className?: string }>(); // className može biti undefined
  const [students, setStudents] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchStudents = async () => {
      if (!className) {
        setError('Naziv razreda nije pronađen u URL-u.');
        setLoading(false);
        return;
      }

      const classId = classMapping[className as keyof typeof classMapping]; // Provereno postojanje ključa

      if (!classId) {
        setError('Nevalidan naziv razreda.');
        setLoading(false);
        return;
      }

      try {
        const response = await agent.Students.getStudentsByClass(classId);
        setStudents(response);
      } catch (error) {
        setError('Došlo je do greške prilikom učitavanja podataka.');
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    fetchStudents();
  }, [className]);

  if (loading)
    return (
      <div className="text-center text-lg font-semibold">Učitavanje...</div>
    );
  if (error)
    return <div className="text-red-500 text-center font-medium">{error}</div>;

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h2 className="text-2xl font-bold text-center text-gray-800 mb-6">
        Učenici u razredu {className}
      </h2>

      {students.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6">
          {students.map((student) => (
            <div
              key={student.id}
              className="bg-white shadow-md rounded-xl p-4 flex flex-col items-center border hover:shadow-lg transition"
            >
              <div className="w-16 h-16 bg-blue-500 text-white flex items-center justify-center rounded-full text-xl font-bold">
                {student.firstName.charAt(0)}
                {student.lastName.charAt(0)}
              </div>
              <h3 className="mt-3 text-lg font-semibold text-gray-700">
                {student.firstName} {student.lastName}
              </h3>
              <p className="text-gray-500 text-sm">ID: {student.id}</p>
            </div>
          ))}
        </div>
      ) : (
        <p className="text-center text-gray-600">
          Nema studenata u ovom razredu.
        </p>
      )}
    </div>
  );
};

export default Statistika;
