import flatpickr from 'flatpickr';
import { useEffect } from 'react';
import 'flatpickr/dist/flatpickr.min.css';
import 'flatpickr/dist/themes/material_blue.css'; // Možete promeniti temu po želji

const TimePickerOne = () => {
  useEffect(() => {
    // Init flatpickr for time picker
    flatpickr('.form-timepicker', {
      enableTime: true,
      noCalendar: true,
      dateFormat: 'H:i',
      time_24hr: true,
      disableMobile: true, // Onemogućava mobilni prikaz
      prevArrow:
        '<svg class="w-4 h-4 text-gray-500" viewBox="0 0 24 24"><path d="M15.293 6.293a1 1 0 011.414 1.414L12.414 12l4.293 4.293a1 1 0 01-1.414 1.414l-5-5a1 1 0 010-1.414l5-5z"/></svg>',
      nextArrow:
        '<svg class="w-4 h-4 text-gray-500" viewBox="0 0 24 24"><path d="M8.707 6.293a1 1 0 00-1.414 1.414L11.586 12 7.293 16.293a1 1 0 001.414 1.414l5-5a1 1 0 000-1.414l-5-5z"/></svg>',
      // Postavke za stilizaciju
    });
  }, []);

  return (
    <div className="relative">
      <label className="mb-2 block text-sm font-medium text-gray-700">
        Time Picker
      </label>
      <input
        className="form-timepicker w-full rounded border border-gray-300 bg-white px-4 py-2 text-gray-700 focus:border-blue-500 focus:outline-none"
        placeholder="HH:MM"
        type="text"
      />
      <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none">
        <svg
          className="w-5 h-5 text-gray-500"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M12 4.5v7h6.5m-8 0H12m0 0v-7m0 7h1.5m4.5-7v6m0 0v1.5m0-1.5h-6m6-6v6m-6-6v6m6-6h-6m6-6v6m6-6v6m-6 6v-6"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </svg>
      </div>
    </div>
  );
};

export default TimePickerOne;
