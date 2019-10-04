<template src="./Inbox.html">
</template>

<script src="./Inbox.js"></script>
<style>

.el-popper{
    margin-top: -46px !important;
   
}
::-webkit-scrollbar { 
    display: none; 
}
.fa.fa-refresh:hover{
     cursor: pointer;
     color:#409EFF;
}
.table-inbox {
    border: 1px solid #d3d3d3;
    margin-bottom: 0;
}
.table-inbox tr td {
    padding: 12px !important;
}
.table-inbox tr td:hover {
    cursor: default;
}
.table-inbox tr td .fa-star.inbox-started, .table-inbox tr td .fa-star:hover {
    color: #f78a09;
}
.table-inbox tr td .fa-star {
    color: #d5d5d5;
}
.table-inbox tr.unread td {
    background: none repeat scroll 0 0 #f7f7f7;
    font-weight: 600;
}

.box-footer {
    border-top-left-radius: 0;
    border-top-right-radius: 0;
    border-bottom-right-radius: 3px;
    border-bottom-left-radius: 3px;
    border-top: 1px solid #f4f4f4;
    padding: 10px;
    background-color: #fff;
}

.list-unstyled, .chart-legend, .contacts-list, .users-list, .mailbox-attachments {
    list-style: none;
    margin: 0;
    padding: 0;
}

.mailbox-attachment-icon {
    text-align: center;
    font-size: 20px;
    color: #666;
    padding: 20px 10px;
}
.mailbox-attachments li {
    float: right;
    width: 200px;
    border: 1px solid #eee;
    margin-bottom: 10px;
    margin-right: 10px;
}
.mailbox-attachment-icon.has-img {
    padding: 0;
}
.el-message {
width:260px;
}


.el-input__inner {
    -webkit-appearance: none;
    background-color: #fff;
    background-image: none;
    border-radius: 4px;
    border: 1px solid #dcdfe6;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    color: #606266;
    display: inline-block;
    font-size: inherit;
    height: 34px;
    line-height: 1;
    outline: 0;
    padding: 0 15px;
    -webkit-transition: border-color .2s cubic-bezier(.645,.045,.355,1);
    transition: border-color .2s cubic-bezier(.645,.045,.355,1);
    width: 100%;
}

 .el-input__icon {
    height: 100%;
    /* width: 25px; */
    text-align: center;
    -webkit-transition: all .3s;
    transition: all .3s;
    /* line-height: 40px; */
    font-size: 21px;
}



</style>