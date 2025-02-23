import React, { useEffect, useState } from "react";
import { NavLink } from "react-router-dom";
import axios from "axios";
import { useSelector } from "react-redux";
import { RootState } from "../store"; // Import RootState type

const HomePage: React.FC = () => {
  const [schoolClasses, setSchoolClasses] = useState<any[]>([]);  // State to hold the classes
  const [loading, setLoading] = useState<boolean>(true);  // State to manage loading state
  const [error, setError] = useState<string>("");  // State to manage error messages

  const user = useSelector((state: RootState) => state.auth.user);  // Access user from the Redux state

  // Fetch data from the API
  useEffect(() => {
    const fetchSchoolClasses = async () => {
      try {
        const response = await axios.get("http://localhost:5024/api/SchoolClass/summaries");  // Replace with your API endpoint
        setSchoolClasses(response.data);  // Set data to state
      } catch (err) {
        setError("Došlo je do greške pri učitavanju razreda.");
        console.error("Error fetching school classes:", err);
      } finally {
        setLoading(false);  // Set loading to false after the request completes
      }
    };

    fetchSchoolClasses();
  }, []);  // Empty dependency array, will run on component mount

  // Handle no classes or error
  if (loading) return <p className="text-gray-500 text-center mt-6">Učitavanje...</p>;
  if (error) return <p className="text-red-500 text-center mt-6">{error}</p>;
  if (schoolClasses.length === 0) return <p className="text-gray-500 text-center mt-6">Nema dostupnih razreda.</p>;

  return (
    <div className="max-w-6xl mx-auto mt-8 p-6 bg-blue-900 text-white rounded-xl shadow-xl">
      <h1 className="text-3xl font-bold mb-6 text-center">
        {user ? `Dobrodošli, ${user.firstName} ${user.lastName}` : "Niste ulogovani"}
      </h1>

      {/* Show different content based on user status */}
      {user ? (
        <div>
          <p className="text-center mb-6">Vaši razredi:</p>
          <div className="overflow-x-auto">
            <table className="w-full text-sm text-left rtl:text-right text-gray-300">
              <thead className="text-xs text-gray-200 uppercase bg-blue-800 dark:bg-blue-700 dark:text-gray-300">
                <tr>
                  <th className="px-8 py-4">Razred</th>
                  <th className="px-8 py-4 text-center">Ukupan skor</th>
                  <th className="px-8 py-4 text-center">Broj učenika</th>
                  <th className="px-8 py-4 text-center">Akcija</th>
                </tr>
              </thead>
              <tbody>
                {schoolClasses.map((classItem) => (
                  <tr key={classItem.id} className="odd:bg-blue-800 even:bg-blue-700 hover:bg-blue-600 transition-all">
                    <td className="px-8 py-5">{classItem.className}</td>
                    <td className="px-8 py-5 text-center">{classItem.totalScore}</td>
                    <td className="px-8 py-5 text-center">{classItem.studentCount}</td>
                    <td className="px-8 py-5 text-center">
                      <NavLink
                        to={`/class/${classItem.className}`}
                        className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition-all"
                      >
                        Uđi
                      </NavLink>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      ) : (
        <p className="text-center text-red-500">Niste ulogovani. Prijavite se za pristup.</p>
      )}
    </div>
  );
};

export default HomePage;
