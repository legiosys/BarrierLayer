Vue.use(VueRouter);
Vue.use(httpVueLoader);

const routes = [
    {
        path: '/',
        component: { template: '<div>Barrier UI</div>' }
    },
    {
        path: '/ui/guest/:id',
        component: httpVueLoader('/components/guest/guest.vue'),
        props: true
    }
];

let router = new VueRouter({
    mode: 'history',
    routes: routes
});

var app = new Vue({
    el: '#app',
    router
});