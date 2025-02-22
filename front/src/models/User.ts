export interface TeachingClass {
  id: number; // Pravi classId
  name: string; // Ime klase (I1, I2 itd.)
}

export interface User {
  email: string;
  token: string;
  firstName: string;
  lastName: string;
  userName: string;
  roles?: string[];
  id: string;
  jmbg: string;
  schoolClassId: number | null; // Stvarni classId
  schoolClassName: string | null;
  teachingClasses: TeachingClass[]; // Sada sadrži i ID i ime klase
}
