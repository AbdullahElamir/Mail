import axios from 'axios';

axios.defaults.headers.common['X-CSRF-TOKEN'] = document.querySelector('meta[name="csrf-token"]').getAttribute('content');




export default {

    loginUserAccount(user) {       
        return axios.post(`/Security/loginUser`, user);
    },
    ResetPassword(email) {
        return axios.post(`/Security/ResetPassword/${email}`);
    }  
}