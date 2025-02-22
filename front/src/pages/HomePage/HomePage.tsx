import React from 'react';
import { useAppSelector } from '../../store/configureStore';
import ClassItem from '../../components/ClassItem';

const HomePage: React.FC = () => {
  const { user } = useAppSelector((state) => state.auth);

  // Loguj user teachingClasses
  console.log(user?.teachingClasses);

  return (
    <div>
      {user?.teachingClasses && user.teachingClasses.length > 0 ? (
        user.teachingClasses.map((classItem) => (
          <ClassItem
            key={classItem} // Koristi id kao key
            title={classItem} // Dodeljujemo samo name kao string
            classId={classItem} // Dodeljujemo id kao classId
          />
        ))
      ) : (
        <p>Nemate dodeljene nastave.</p>
      )}
    </div>
  );
};

export default HomePage;
