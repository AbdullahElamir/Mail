import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');



export default {
    //Login(loginName, password, secretNo) {
    //    return axios.post(baseUrl + '/security/login', { loginName, password, secretNo });
    //},
    //Logout() {
    //    return axios.post(baseUrl + '/security/logout');
    //},    
    //CheckLoginStatus() {
    //    return axios.post('/security/checkloginstatus');
    //},  
  
    //*******************************************  Branches Service *********************************
    GetBranches(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branches/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
      DeleteBranch(BracnhId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
          return axios.post(`/Api/Admin/Branches/${BracnhId}/delete`);
    },
    //GetCompanies_v1(pageNo, pageSize) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.get(baseUrl + `/Admin/Companies/GetCompanies?pageno=${pageNo}&pagesize=${pageSize}`);
    //},
    //DeleteCompany(CompanyId) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/${CompanyId}/delete`);
    //},
    //AddCompany(Company) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/Add`, Company);
    //},
    //EditCompany(Company) {
    //    axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
    //    return axios.post(baseUrl + `/admin/Companies/Edit`, Company);
    //},




}