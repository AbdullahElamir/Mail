export default {
    name: 'AppHeader',    
    created() { 
        
    },
    data() {
        return {            
            loginDetails: null,
            ProfileMenu: 'dropdown-menu settings-menu dropdown-menu-right',
            active:1
        };
    },
  
    methods: {

        Logout() { 
            this.$http.logout()
                .then(response => {
                    window.location.href='/Security/Login';
                })
                .catch((err) => {
                    console.error(err);
                });
        },

        href(path) {
            if (this.ProfileMenu === 'dropdown-menu settings-menu dropdown-menu-right') {
                this.ProfileMenu = 'dropdown-menu settings-menu dropdown-menu-right show';
            } else {
                this.ProfileMenu = 'dropdown-menu settings-menu dropdown-menu-right';
            }
            this.$router.push(path);
        },

        OpenSubMenu() {
            if (this.ProfileMenu === 'dropdown-menu settings-menu dropdown-menu-right') {
                this.ProfileMenu = 'dropdown-menu settings-menu dropdown-menu-right show';
            } else {
                this.ProfileMenu = 'dropdown-menu settings-menu dropdown-menu-right';
            }
        },

        HideDashboard() {
            var root = document.getElementById("Body");
            if (root.getAttribute('class') == 'app sidebar-mini rtl pace-done sidenav-toggled') {
                root.setAttribute('class', 'app sidebar-mini rtl pace-done');
            } else {
                root.setAttribute('class', 'app sidebar-mini rtl pace-done sidenav-toggled');
            }
        },

        // ********************** Template InterActive ***********
        OpenMenuByToggle() {
            var root = document.getElementsByTagName('html')[0]; // '0' to assign the first (and only `HTML` tag)
            if (root.getAttribute('class') == 'nav-open') {
                root.setAttribute('class', '');
            } else {
                root.setAttribute('class', 'nav-open');
            }
        },
        OpenNotificationMenu() {
            var root = document.getElementById("Notifications");
            if (root.getAttribute('class') == 'dropdown open') {
                root.setAttribute('class', 'dropdown');
            } else if (root.getAttribute('class') == 'dropdown') {
                root.setAttribute('class', 'dropdown open');
            }
        }
        //****************************************************************

      
    }    
}
