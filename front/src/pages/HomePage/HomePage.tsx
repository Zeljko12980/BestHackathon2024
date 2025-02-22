import React, { useEffect, useState } from 'react';
import { useAppSelector } from '../../store/configureStore';
import ClassItem from '../../components/ClassItem';

const HomePage: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const { user } = useAppSelector((state) => state.auth);

  // Kada korisnik ima nastavu, mapiraj kroz "teachingClasses" i prikaži "ClassItem" za svaki razred
  return (
    <div>
      {/* Ako korisnik ima teachingClasses, mapiraj kroz njih i renderuj ClassItem */}
      {user?.teachingClasses && user.teachingClasses.length > 0 ? (
        user.teachingClasses.map((className, index) => (
          <ClassItem
            key={index}
            title={className} // Ovde je samo string (ime razreda)
            classId={className} // Koristimo ime razreda kao ID za URL
          />
        ))
      ) : (
        <p>Nemate dodeljene nastave.</p>
      )}
    </div>
  );
};

export default HomePage;
