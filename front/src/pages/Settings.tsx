import { useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import { useAppSelector } from '../store/configureStore';
import agent from '../data/agent';
import { useNavigate } from 'react-router-dom';
import UserNotLoggedIn from '../components/UserNotLoggedIn';

const Settings = () => {
  const { user } = useAppSelector((state) => state.auth);
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    FirstName: user?.firstName || '',
    LastName: user?.lastName || '',
    Email: user?.email || '',
    UserName: user?.userName || '',
    AdresaPrebivalista: user?.adresaPrebivalista || '',
    OpstinaPrebivalista: user?.opstinaPrebivalista || '',
  });

  console.log(user);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    try {
      const response = await agent.Auth.updateUser(formData, user?.token);
      console.log('update ' + response);
      alert('Podaci su uspešno ažurirani!');
      navigate('/');
    } catch (error) {
      alert('Došlo je do greške pri ažuriranju podataka.');
    }
  };

  if (!user) return <UserNotLoggedIn />;

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="max-w-3xl w-full p-6 bg-white shadow-lg rounded-lg">
        <Breadcrumb pageName="Podešavanja naloga" />
        <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
          <div className="border-b border-stroke py-4 px-7 dark:border-strokedark">
            <h3 className="font-medium text-black dark:text-white">
              Lični podaci
            </h3>
          </div>
          <div className="p-7">
            <form onSubmit={handleSubmit}>
              <div className="mb-5.5 flex flex-col gap-5.5 sm:flex-row">
                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="firstName"
                  >
                    Ime
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="FirstName"
                    id="FirstName"
                    placeholder=""
                    value={formData.FirstName}
                    onChange={handleChange}
                  />
                </div>

                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="lastName"
                  >
                    Prezime
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="LastName"
                    id="LastName"
                    placeholder=""
                    value={formData.LastName}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="mb-5.5 flex flex-col gap-5.5 sm:flex-row">
                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="email"
                  >
                    Email
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="Email"
                    id="Email"
                    placeholder=""
                    value={formData.Email}
                    onChange={handleChange}
                  />
                </div>

                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="userName"
                  >
                    Korisničko ime
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="UserName"
                    id="UserName"
                    placeholder=""
                    value={formData.UserName}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="mb-5.5 flex flex-col gap-5.5 sm:flex-row">
                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="userName"
                  >
                    Adresa Prebivališta
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="AdresaPrebivalista"
                    id="AdresaPrebivalista"
                    placeholder=""
                    value={formData.AdresaPrebivalista}
                    onChange={handleChange}
                  />
                </div>

                <div className="w-full sm:w-1/2">
                  <label
                    className="mb-3 block text-sm font-medium text-black dark:text-white"
                    htmlFor="userName"
                  >
                    Opština prebivališta
                  </label>
                  <input
                    className="w-full rounded border border-stroke bg-gray py-3 px-4.5 text-black focus:border-primary focus-visible:outline-none dark:border-strokedark dark:bg-meta-4 dark:text-white dark:focus:border-primary"
                    type="text"
                    name="OpstinaPrebivalista"
                    id="OpstinaPrebivalista"
                    placeholder=""
                    value={formData.OpstinaPrebivalista}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <button
                type="submit"
                className="mt-5 w-full rounded bg-primary py-3 px-4.5 text-white"
              >
                Save Changes
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Settings;
