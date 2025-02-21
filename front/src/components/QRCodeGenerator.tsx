import QRCode from 'react-qr-code';

const QRCodeGenerator = () => {
  const localIPAddress = 'http://10.0.10.121:5173'; // Zameni sa svojom lokalnom IP adresom i portom

  return (
    <div style={{ textAlign: 'center', marginTop: '50px' }}>
      <h2>Scan QR Code to Open App on Mobile</h2>
      <QRCode value={localIPAddress} size={256} />
      <p>Open the app by scanning this QR code with your mobile device.</p>
    </div>
  );
};

export default QRCodeGenerator;
