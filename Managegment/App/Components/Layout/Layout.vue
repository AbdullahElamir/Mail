<template src="./Layout.html"></template>

<script src="./Layout.js"></script>
<style scoped>


    .sidebar {
        position: fixed;
        top: 0;
        bottom: 0;
        left: auto;
        right: 0;
        width: 231px;
        display: block;
        z-index: 1;
        color: #fff;
        font-weight: 200;
        background-size: cover;
        background-position: center center;
    }
        .sidebar:after {
           
            background-size: 150% 150%;
            z-index: 3;
            opacity: 1;
        }
        .sidebar:before {
            opacity: 2.33;
            background: #1f69b67d;
        }
</style>
  