export interface User {
  email: string;
  token: string;
  firstName: string;
  lastName: string;
  userName: string;
  roles?: string[];
  id: string;
  jmbg: string;
  schoolClassId: string | null; // Ako je ovo nullable, može biti null
  schoolClassName: string | null; // Isto kao gore
  teachingClasses: string[]; // Niz časova koje korisnik predaje
}
