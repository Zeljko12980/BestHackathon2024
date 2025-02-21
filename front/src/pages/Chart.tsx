import React, { useState } from 'react';
import Breadcrumb from '../components/Breadcrumbs/Breadcrumb';
import ChartOne from '../components/Charts/ChartOne';
import ChartThree from '../components/Charts/ChartThree';
import agent from '../data/agent';

const Chart: React.FC = () => {
  const token = localStorage.getItem('token') || '';

  const handleCsvExport = async () => {
    try {
      const response = await agent.FileExport.exportCsv(token);
      const blob = new Blob([response], { type: 'text/csv' });
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'appointments.csv';
      link.click();
    } catch (error) {
      console.error('Failed to download CSV:', error);
    }
  };

  const handleXlsxExport = async () => {
    try {
      const response = await agent.FileExport.exportXlsx(token);
      const blob = new Blob([response], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'appointments.xlsx';
      link.click();
    } catch (error) {
      console.error('Failed to download XLSX:', error);
    }
  };

  const handlePdfExport = async () => {
    try {
      const response = await agent.FileExport.exportPdf(token);
      const blob = new Blob([response], { type: 'application/pdf' });
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = 'appointments.pdf';
      link.click();
    } catch (error) {
      console.error('Failed to download PDF:', error);
    }
  };
  return (
    <>
      <Breadcrumb pageName="Statistika" />

      {/* ChartOne koji zauzima celu širinu */}
      <div className="w-full mb-6">
        <ChartOne />
      </div>

      {/* ChartThree i RouteMap pored jednog drugog */}
      <div className="flex gap-6">
        <div className="flex-1">
          <ChartThree />
          {/* Dugmad ispod ChartThree */}
          <div className="flex justify-center mt-4">
            <button
              onClick={handleCsvExport}
              className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 mx-2 rounded"
            >
              Preuzmi CSV
            </button>
            <button
              onClick={handleXlsxExport}
              className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 mx-2 rounded"
            >
              Preuzmi XLSX
            </button>
            <button
              onClick={handlePdfExport}
              className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 mx-2 rounded"
            >
              Preuzmi PDF
            </button>
          </div>
        </div>
        {/* <div className="flex-1">
          <RouteMap />
        </div> */}
      </div>
    </>
  );
};

export default Chart;
