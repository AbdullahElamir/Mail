<template src="./MessageDisplay.html">
</template>

<script src="./MessageDisplay.js"></script>

<style>
    
.mailbox-attachment-icon {
    text-align: center;
    font-size: 20px;
    color: #666;
    padding: 20px 10px;

}

.mailbox-attachments li {
    float: right;
    width: auto;
    border: 1px solid #eee;
    margin-bottom: 10px;
    margin-right: 10px;
    height:30px;
}
.mailbox-attachment-icon.has-img {
    padding: 0;
}
.list-unstyled, .chart-legend, .contacts-list, .users-list, .mailbox-attachments {
    list-style: none;
    margin: 0;
    padding: 10px;
}
.attachment-margin {
    margin-top: 4px;
    margin-right: 6px;
}
.btn-custom{
    height: 40px;
    width: auto;
    text-align: center;
    border: 1px solid #cacccc;
}




.user-comment img {
    float: right;
    margin-left: 15px;
    margin-bottom: 10px; 
    border-radius: 50%;
    width: 40px;
    height: 40px;
    border: 2px solid #968c8c;
}
img {
    max-width: 50px;
}

.user-coment2 {
    border-bottom: none;
    padding: 20px 0px;
}
.user-comment {
    border-bottom: 1px solid #e7e7e7;
    padding-bottom: 15px;
}

.comment-details p {
    color: #222;
    font-size: 17px;
    font-weight: 100;
    line-height: 24px;
}
p {
    margin: 0 0 15px;
}

.comment-replay {
    float: left;
    color: grey;
    font-size: 11px;
    text-transform: uppercase;
}
.comment-head h3 {
    margin: 20px 0;
    text-transform: uppercase;
    position: relative;
    text-align: center;
}

.comment-head h3::before, .lead-head h3:before {
    background: #e7e7e7 none repeat scroll 0 0;
    content: "";
    height: 1px;
    left: 0;
    position: absolute;
    top: 50%;
}
.comment-head h3::before, .comment-head h3::after {
    width: 37%;
}
.comment-head h3::after, .lead-head h3:after {
    background: #e7e7e7 none repeat scroll 0 0;
    content: "";
    height: 1px;
    right: 0;
    position: absolute;
    top: 50%;
}



/*/////////////////////////////////////////////*/



 
 
 

.mg-tb-30{
	margin:30px 0px;
}
.mg-b-30{
	margin-bottom:30px;
}
.mg-tb-15{
	margin:15px 0px;
}
.mg-t-30{
	margin-top:30px;
}
.mg-b-10{
	margin-bottom:16px;
}
.mg-t-15{
	margin-top:15px;
}
.mg-b-15{
	margin-bottom:15px;
}
.mt-b-30{
	margin-bottom:30px;
}
/*----------------------------------------*/
/*  8. Menu collapse Area
/*----------------------------------------*/
.all-content-wrapper {
    margin-left: 205px;
    transition: all 0.3s;
}

 
 
 
   
 
 
.hpanel.email-compose {
    background: #fff;
	padding:30px;
}
.hpanel .panel-body.panel-csm{
	border: 1px solid rgba(0,0,0,.12);
}

.mailbox-view .panel-heading {
    padding: 10px 0px;
}
.custom-address-mailbox {
    margin-bottom: 20px;
    padding: 10px;
}
.file-body i.fa {
    font-size: 40px;
}
.incon-ctn-view{
	text-align:center;
}
.btn-success.compose-btn {
    background-color: #e12503;
    border-color: #e12503;
    color: #fff;
}
.panel-body.no-padding{
	box-shadow:none;
}
.panel-body p {
    font-size: 14px;
    color: #444;
    line-height: 22px;
}
.btn-primary.ft-compse {
    background-color: #e12503;
    border-color: #e12503;
    color: #fff;
}
.panel-footer {
    padding: 10px 15px;
    background-color: #f3f3f3;
       border-top: 1px solid #ddd;  
      border-bottom-left-radius: 3px;  
      border-bottom-right-radius: 3px;  
}

.fade-enter-active  {
        transition: opacity 0.5s;
}
    .fade-leave-active {
        transition: opacity 0s;
    }

.fade-enter, .fade-leave-to {
opacity: 0;
}

.el-card {
    border: 1px solid #ebeef5;
    background-color: #fff;
    box-shadow: 0 0px 0px 0 rgba(0,0,0,0);
    color: #303133;
}
.el-card, .el-message {
    /* border-radius: 4px; */
    overflow: hidden;
}




</style>