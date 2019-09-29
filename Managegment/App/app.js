import Vue from 'vue';
import VueI18n from 'vue-i18n';
import VueRouter from 'vue-router';
import ElementUI from 'element-ui';
import Vuetify from 'vuetify';
import locale from 'element-ui/lib/locale/lang/en';
import BlockUIService from './Shared/BlockUIService.js';
import Layout from './Components/Layout/Layout.vue';
import Home from './Components/Home/Home.vue';
import Branches from './Components/Branches/Branches.vue';
import AdTypes from './Components/AdTypes/AdTypes.vue';
import Inbox from './Components/Messages/Inbox/Inbox.vue';
import Sent from './Components/Messages/Sent/Sent.vue';

import Users from './Components/Users/Users.vue';
import EditUsersProfile from './Components/Users/EditUsersProfile/EditUsersProfile.vue';
import ChangePassword from './Components/Users/ChangePassword/ChangePassword.vue';
import NewMessage from './Components/Messages/NewMessage/NewMessage.vue';
import DataService from './Shared/DataService';
import messages from './i18n';



Vue.use(Vuetify);
Vue.use(VueI18n);
Vue.use(VueRouter);
Vue.use(ElementUI,{ locale });

Vue.config.productionTip = false;

Vue.prototype.$http = DataService;
Vue.prototype.$blockUI = BlockUIService;


export const eventBus = new Vue();

const i18n = new VueI18n({
    locale: 'ar', // set locale
    messages, // set locale messages
})

const router = new VueRouter({
    mode: 'history',
    base: __dirname,
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home }, 
        { path: '/Branches', component: Branches },
        { path: '/Inbox', component: Inbox },
        { path: '/Sent', component: Sent }, 
        { path: '/Branches', component: Branches }, 
        { path: '/AdTypes', component: AdTypes }, 
        { path: '/Users', component: Users },
        { path: '/EditUsersProfile', component: EditUsersProfile },
        { path: '/ChangePassword', component: ChangePassword },  
        { path: '/NewMessage', component: NewMessage },
    ]
});

Vue.filter('toUpperCase', function (value) {
    if (!value) return '';
    return value.toUpperCase();
});

new Vue({
    i18n,
    router,
    render: h => {
        return h(Layout);
    }    
}).$mount('#app');
