import { NavLink } from "react-router-dom";

const ClassItem: React.FC<{ title: string; classId: number }> = ({
  title,
  classId,
}) => {
  return (
    <div className="bg-white/80 backdrop-blur-lg border border-gray-200 p-6 rounded-3xl shadow-xl mb-6 transition-all duration-300 transform hover:-translate-y-2 hover:shadow-2xl hover:bg-white">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-extrabold text-gray-900 tracking-wide">
          {title}
        </h2>
        <NavLink
          className="bg-gradient-to-r from-blue-500 to-indigo-600 text-white py-2 px-6 rounded-xl font-semibold shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105 hover:from-indigo-500 hover:to-purple-600 focus:outline-none focus:ring-4 focus:ring-blue-300"
          to={`/class/${classId}`}
        >
          Uđi u čas
        </NavLink>
      </div>
    </div>
  );
};

export default ClassItem;
