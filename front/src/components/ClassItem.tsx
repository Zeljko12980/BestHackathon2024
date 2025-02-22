import { NavLink } from 'react-router-dom';

const ClassItem: React.FC<{ title: string; classId: number }> = ({
  title,
  classId,
}) => {
  return (
    <div className="bg-white p-6 rounded-lg shadow-md mb-4 hover:shadow-xl hover:bg-gray-100 hover:scale-105 transition-all duration-300 transform">
      <div className="flex items-center justify-between">
        <h2 className="text-xl font-semibold text-gray-800">{title}</h2>
        <NavLink
          className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-opacity-50 transition-all duration-300 transform hover:scale-110"
          to={`/class/${classId}`} // Koristimo ID razreda u URL-u
        >
          Uđi u čas
        </NavLink>
      </div>
    </div>
  );
};

export default ClassItem;
