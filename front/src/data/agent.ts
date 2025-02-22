import axios, { AxiosResponse } from 'axios';

axios.defaults.baseURL = 'http://localhost:5024/api/';

const responseBody = (response: AxiosResponse) => response.data;

const request = {
  get: (url: string, token?: string) => {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return axios.get(url, { headers }).then(responseBody);
  },
  getFile: (url: string, token?: string) => {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return axios.get(url, { headers, responseType: 'blob' }).then(responseBody);
  },
  post: (url: string, body: {}, token?: string) => {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return axios.post(url, body, { headers }).then(responseBody);
  },
  put: (url: string, body: {}, token?: string) => {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return axios.put(url, body, { headers }).then(responseBody);
  },
  delete: (url: string, token?: string) => {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    return axios.delete(url, { headers }).then(responseBody);
  },
};

const FileExport = {
  exportCsv: (token?: string) => request.getFile('CSVExport/export', token),
  exportPdf: (token?: string) => request.getFile('PDFExport/export-pdf', token),
  exportXlsx: (token?: string) =>
    request.getFile('XLSXExport/export-xlsx', token),
};

const Auth = {
  login: (values: any, token?: string) =>
    request.post('Auth/login', values, token),
  register: (values: any, token?: string) =>
    request.post('Auth/register', values, token),
  currentUser: (token?: string) => request.get('Auth/currentUser', token),
  updateUser: (userData: any, token?: string) =>
    request.put('Auth/edit', userData, token),
};

const Documents = {
  upload: (file: File, token?: string) => {
    const formData = new FormData();
    formData.append('image', file);

    return request.post('Document/upload', formData, token);
  },
};

const agent = {
  Auth,
  FileExport,
  Documents,
};

export default agent;
