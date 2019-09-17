import axios from 'axios';
const baseUrl = '/Api';
const userUrl = '/';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');



export default {
    Login(loginName, password, secretNo) {
        return axios.post(baseUrl + '/security/login', { loginName, password, secretNo });
    },
    Logout() {
        return axios.post(baseUrl + '/security/logout');
    },    
    //CheckLoginStatus() {
    //    return axios.post('/security/checkloginstatus');
    //},  

    GetLoginInfo() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(baseUrl + `/Admin/User/GetLoginInfo`);
    },
    logout(user) {
        return axios.post(`/Security/Logout`);
    },

    AddUser(User) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post( 'api/admin/User/AddUser', User);
    },
    ActivateUser(UserId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(baseUrl + `/admin/User/${UserId}/Activate`);
    },
    DeactivateUser(UserId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(baseUrl + `/admin/User/${UserId}/Deactivate`);
    },
    EditUser(User) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        console.log(User);
        return axios.post(baseUrl + '/Admin/User/EditUser', User);
    },
    EditUsersProfile(User) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');

        return axios.post(baseUrl + '/Admin/User/EditUsersProfile', User);
    },
    GetUsers(pageNo, pageSize, UserType) {
        console.log(UserType);
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(baseUrl + `/admin/User/GetUsers?pageno=${pageNo}&pagesize=${pageSize}&UserType=${UserType}`);
    },
    UploadImage(obj) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(baseUrl + '/Admin/User/UploadImage', obj);
    },

    ChangePassword(userPassword) {
        return axios.post(`/Security/ChangePassword`, userPassword);
    },
    //*******************************************  Branches Service *********************************
    GetBranches(pageNo, pageSize) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branches/Get?pageno=${pageNo}&pagesize=${pageSize}`);
    },
    GetBranchsV1() {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.get(`/Api/Admin/Branches/GetBranchs`);
    },
  
    DeleteBranch(BracnhId) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branches/${BracnhId}/delete`);
    },
    AddBranches(Branch) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branches/Add`, Branch);
    },

    EditBranches(Branch) {
        axios.defaults.headers.common['Authorization'] = 'Bearer ' + document.querySelector('meta[name="api-token"]').getAttribute('content');
        return axios.post(`/Api/Admin/Branches/Edit`, Branch);
    },
   
 



}