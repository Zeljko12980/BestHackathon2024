import React, { useState } from 'react';

interface Student {
  order: number;
  name: string;
  points: number;
}

const studentsData: Student[] = [
  { order: 1, name: 'John Doe', points: 95 },
  { order: 2, name: 'Jane Smith', points: 60 },
  { order: 3, name: 'Sam Johnson', points: 40 },
  { order: 4, name: 'Emily Davis', points: 75 },
];

const TableStudents: React.FC = () => {
  const [students, setStudents] = useState<Student[]>(studentsData);
  const [sortConfig, setSortConfig] = useState<{
    key: string;
    direction: 'ascending' | 'descending';
  }>({
    key: 'points',
    direction: 'ascending',
  });

  // Funkcija koja vraća odgovarajuću boju pozadine na osnovu bodova
  const getPointsBackgroundColor = (points: number) => {
    if (points >= 90) return 'bg-green-500';
    if (points >= 60) return 'bg-orange-500';
    return 'bg-red-500';
  };

  // Funkcija za sortiranje studenata po bodovima
  const sortedStudents = React.useMemo(() => {
    const sortableStudents = [...students];
    sortableStudents.sort((a, b) => {
      if (
        a[sortConfig.key as keyof Student] < b[sortConfig.key as keyof Student]
      ) {
        return sortConfig.direction === 'ascending' ? -1 : 1;
      }
      if (
        a[sortConfig.key as keyof Student] > b[sortConfig.key as keyof Student]
      ) {
        return sortConfig.direction === 'ascending' ? 1 : -1;
      }
      return 0;
    });
    return sortableStudents;
  }, [students, sortConfig]);

  // Funkcija koja menja smjer sortiranja
  const requestSort = () => {
    let direction: 'ascending' | 'descending' = 'ascending';
    if (sortConfig.key === 'points' && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key: 'points', direction });
  };

  // Funkcija koja renderuje ikonu sortiranja
  const getSortIcon = () => {
    if (sortConfig.direction === 'ascending') {
      return (
        <svg
          className="h-5 w-5 text-gray-500"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth="2"
            d="M19 9l-7 7-7-7"
          />
        </svg>
      );
    }
    return (
      <svg
        className="h-5 w-5 text-gray-500"
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke="currentColor"
      >
        <path
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          d="M5 15l7-7 7 7"
        />
      </svg>
    );
  };

  return (
    <div className="rounded-sm border border-stroke bg-white px-5 pt-6 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
      <div className="max-w-full overflow-x-auto">
        <div className="flex justify-end mb-4">
          <button
            onClick={requestSort}
            className="flex items-center space-x-2 text-black dark:text-white hover:text-primary"
          >
            <span>Sort</span>
            {getSortIcon()}
          </button>
        </div>
        <table className="w-full table-auto">
          <thead>
            <tr className="bg-gray-200 text-left dark:bg-meta-4">
              <th className="min-w-[50px] py-4 px-4 font-medium text-black dark:text-white">
                Broj u dnevniku
              </th>
              <th className="py-4 px-4 font-medium text-center text-black dark:text-white">
                Ime i Prezime
              </th>
              <th className="min-w-[120px] py-4 px-8 font-medium text-black dark:text-white text-right">
                Poeni
              </th>
            </tr>
          </thead>
          <tbody>
            {sortedStudents.map((student, index) => (
              <tr key={index}>
                <td className="border-b border-[#eee] py-5 px-4 pl-9 dark:border-strokedark xl:pl-11">
                  <p className="text-black dark:text-white">{student.order}</p>
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark text-center">
                  <p className="text-black dark:text-white">{student.name}</p>
                </td>
                <td className="border-b border-[#eee] py-5 px-4 dark:border-strokedark text-right pr-9">
                  <p
                    className={`inline-flex rounded-full py-1 px-3 text-sm font-medium text-white ${getPointsBackgroundColor(student.points)}`}
                  >
                    {student.points}
                  </p>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default TableStudents;
