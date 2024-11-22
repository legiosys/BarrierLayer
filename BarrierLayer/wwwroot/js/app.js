Vue.use(VueRouter);
Vue.use(httpVueLoader);
Vue.use(VueClipboard);

const routes = [
    {
        path: '/barrier/',
        component: { template: '<div>Barrier UI</div>' }
    },
    {
        path: '/barrier/ui/admin/guest',
        component: httpVueLoader('/barrier/components/guest/admin.vue')
    },
    {
        path: '/barrier/ui/guest/:id',
        component: httpVueLoader('/barrier/components/guest/guest.vue'),
        props: true
    }
];

let router = new VueRouter({
    mode: 'history',
    routes: routes,
    base: window.appPrefix
});

let app = new Vue({
    el: '#app',
    router
});