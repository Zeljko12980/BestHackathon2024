import React from 'react';
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Button,
} from '@mui/material';

interface ConfirmDialogProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
}

const ConfirmDialog: React.FC<ConfirmDialogProps> = ({
  open,
  onClose,
  onConfirm,
}) => {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Potvrdite brisanje</DialogTitle>
      <DialogContent>
        Da li ste sigurni da želite da obrišete ovaj termin?
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          Otkaži
        </Button>
        <Button onClick={onConfirm} color="error">
          Obriši
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ConfirmDialog;
