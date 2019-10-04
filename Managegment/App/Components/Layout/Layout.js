import AppHeader from './AppHeader/AppHeader.vue';
import AppFooter from './AppFooter/AppFooter.vue';
import Dashboard from './Dashboard/Dashboard.vue';
import 'expose-loader?$!expose-loader?jQuery!jquery';

export default {
    
    name: 'layout',   
    
    components: 
    {
        'app-header': AppHeader,
        'app-footer': AppFooter,
        'app-dasboard': Dashboard,
    },
    
    created() 
    {
        this.$blockUI.$loading = this.$loading;
        var loginDetails = sessionStorage.getItem('currentUser');
        
        this.loginDetails = JSON.parse(loginDetails);
        
        if (loginDetails != null) 
        {
            this.loginDetails = JSON.parse(loginDetails);
        } 
        else 
        {
            window.location.href = '/Security/Login';
        }

        $(document).ready(function ($) {
            "use strict";

	var treeviewMenu = $('.app-menu');

	// Toggle Sidebar
	$('[data-toggle="sidebar"]').click(function(event) {
		event.preventDefault();
		$('.app').toggleClass('sidenav-toggled');
	});

	// Activate sidebar treeview toggle
	$("[data-toggle='treeview']").click(function(event) {
		event.preventDefault();
		if(!$(this).parent().hasClass('is-expanded')) {
			treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
		}
		$(this).parent().toggleClass('is-expanded');
	});

	// Set initial active toggle
	$("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

	//Activate bootstrip tooltips
	//$("[data-toggle='tooltip']").tooltip();


          });


    },
   
    data() {
        return {
            isAuthenticated: false,
            isActive: false
        };
    },
    methods: {
        href(path) {
            this.$router.push(path);
        }


      


    }
    
}
