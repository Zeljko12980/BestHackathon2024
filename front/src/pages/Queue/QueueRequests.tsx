import React, { useEffect, useState } from 'react';
import agent from '../../data/agent';
import { Check } from '@mui/icons-material';
import { useAppSelector } from '../../store/configureStore';

const categories: Record<string, string[]> = {
  'Zahtjevi za dozvole': [
    'Građevinska dozvola',
    'Vozačka dozvola',
    'Dozvola za rad',
  ],
};

const QueueRequests: React.FC = () => {
  const [appointments, setAppointments] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const user = useAppSelector((state) => state.auth.user);

  useEffect(() => {
    const fetchAppointments = async () => {
      try {
        const response = await agent.Appointments.getAll();
        setAppointments(response);
      } catch (err: any) {
        setError(
          err.message || 'Došlo je do greške prilikom preuzimanja termina.',
        );
      } finally {
        setLoading(false);
      }
    };

    fetchAppointments();
  }, []);

  const handleDelete = async (appointmentId: number) => {
    try {
      await agent.Appointments.removeAppointment(appointmentId, user?.token);
      alert('Termin je uspešno završen.');
      window.location.reload();
    } catch (error) {
      console.error('Greška pri obradi termina:', error);
      alert('Došlo je do greške prilikom obrade termina.');
    } finally {
      setLoading(false);
    }
  };

  const groupedAppointments = categories['Zahtjevi za dozvole'].reduce(
    (acc, serviceSubType) => {
      const appointmentsForServiceSubType = appointments
        .filter(
          (appointment: any) => appointment.serviceSubType === serviceSubType,
        )
        .sort((a, b) => {
          const dateTimeA = new Date(a.appointmentDate).getTime();
          const dateTimeB = new Date(b.appointmentDate).getTime();

          if (dateTimeA !== dateTimeB) {
            return dateTimeA - dateTimeB;
          }

          const timeA = new Date(`1970-01-01T${a.appointmentTime}`).getTime();
          const timeB = new Date(`1970-01-01T${b.appointmentTime}`).getTime();
          return timeA - timeB;
        });

      if (appointmentsForServiceSubType.length > 0) {
        acc[serviceSubType] = appointmentsForServiceSubType;
      }
      return acc;
    },
    {} as Record<string, any[]>,
  );

  if (loading)
    return <div className="text-center text-gray-600">Učitavanje...</div>;
  if (error)
    return (
      <div className="text-center text-red-600 font-semibold">
        Greška: {error}
      </div>
    );

  return (
    <div className="container mx-auto p-4">
      <h2 className="text-2xl font-bold mb-4">Red za čekanje dokumenata</h2>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {Object.keys(groupedAppointments).map((subcategory) => (
          <div
            key={subcategory}
            className="bg-white p-4 shadow-md rounded-md border border-gray-300 relative"
          >
            <h3 className="text-xl font-semibold mb-4">{subcategory}</h3>
            {groupedAppointments[subcategory].map((appointment: any) => (
              <div
                key={appointment.appointmentId}
                className="bg-gray-50 p-3 mb-4 rounded-md shadow-sm relative"
              >
                {user && user.roles?.includes('Admin') && (
                  <button
                    onClick={() => handleDelete(appointment.appointmentId)}
                    className="absolute top-2 right-2 text-green-500 hover:text-green-700"
                    aria-label="Obriši termin"
                  >
                    <Check />
                  </button>
                )}
                <div className="text-lg font-semibold mb-2">
                  {appointment.userEmail}
                </div>
                <div className="text-sm text-gray-600">
                  Datum:{' '}
                  {new Date(appointment.appointmentDate).toLocaleDateString()}
                </div>
                <div className="text-sm text-gray-600">
                  Vreme: {appointment.appointmentTime}
                </div>
              </div>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
};

export default QueueRequests;
