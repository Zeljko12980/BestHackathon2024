import axios from 'axios';
import React, { useState, useEffect } from 'react';

interface Student {
  name: string;
  order: number;
  score: number;
  schoolClass: string | null;
  schoolClassId: string;
}

const TableStudents: React.FC = () => {
  const [students, setStudents] = useState<Student[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [sortConfig, setSortConfig] = useState<{
    key: string;
    direction: 'ascending' | 'descending';
  }>({
    key: 'score',
    direction: 'ascending',
  });

  // Funkcija za formatiranje imena razreda
  const getClassLabel = (classId: number): string => {
    switch (classId) {
      case 1:
        return 'I-1';
      case 2:
        return 'I-2';
      case 3:
        return 'I-3';
      case 4:
        return 'IV-1';
      case 5:
        return 'IV-2';
      case 6:
        return 'IV-3';
      default:
        return 'N/A'; // Ukoliko nije poznat razred
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          'http://localhost:5024/api/students/getAllStudents',
        );
        console.log('response: ', response);

        const mappedStudents: Student[] = response.data.value.map(
          (student: any) => ({
            name: `${student.firstName} ${student.lastName}`,
            score: student.score || 0,
            schoolClassId: student.schoolClassId,
          }),
        );

        const sortedStudents = mappedStudents.sort((a, b) => b.score - a.score);
        const orderedStudents = sortedStudents.map((student, index) => ({
          ...student,
          order: index + 1,
        }));

        setStudents(orderedStudents);
        console.log(orderedStudents);
      } catch (err) {
        console.log(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

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
        return sortConfig.direction === 'descending' ? 1 : -1;
      }
      return 0;
    });
    return sortableStudents;
  }, [students, sortConfig]);

  const requestSort = () => {
    let direction: 'ascending' | 'descending' = 'ascending';
    if (sortConfig.key === 'score' && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key: 'score', direction });
  };

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

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div className="rounded-lg border border-gray-300 bg-white px-6 py-4 shadow-lg dark:border-stroke-dark dark:bg-gray-800">
      <div className="max-w-full overflow-x-auto">
        <div className="flex justify-end mb-4">
          <button
            onClick={requestSort}
            className="flex items-center space-x-2 text-black dark:text-white hover:text-blue-500"
          >
            <span>Sortiraj po poenima</span>
            {getSortIcon()}
          </button>
        </div>
        <table className="w-full table-auto text-sm">
          <thead>
            <tr className="bg-gray-200 text-left dark:bg-gray-700">
              <th className="py-3 px-4 font-semibold text-center text-gray-700 dark:text-white">
                Mjesto
              </th>
              <th className="py-3 px-4 font-semibold text-center text-gray-700 dark:text-white">
                Ime i Prezime
              </th>
              <th className="py-3 px-4 font-semibold text-center text-gray-700 dark:text-white">
                Razred
              </th>
              <th className="py-3 px-4 font-semibold text-center text-gray-700 dark:text-white">
                Poeni
              </th>
            </tr>
          </thead>
          <tbody>
            {sortedStudents.map((student, index) => (
              <tr
                key={index}
                className="hover:bg-gray-100 dark:hover:bg-gray-600"
              >
                <td className="border-b py-4 px-4 text-center text-gray-600 dark:text-white">
                  {student.order}
                </td>
                <td className="border-b py-4 px-4 text-center text-gray-600 dark:text-white">
                  {student.name}
                </td>
                <td className="border-b py-4 px-4 text-center text-gray-600 dark:text-white">
                  {getClassLabel(Number(student.schoolClassId))}
                </td>
                <td className="border-b py-4 px-4 text-center text-gray-600 dark:text-white">
                  {student.score}
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
