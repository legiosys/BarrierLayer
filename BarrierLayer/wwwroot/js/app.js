Vue.use(VueRouter);
Vue.use(httpVueLoader);
Vue.use(VueClipboard);

const routes = [
    {
        path: '~/',
        component: { template: '<div>Barrier UI</div>' }
    },
    {
        path: '~/ui/admin/guest',
        component: httpVueLoader('/components/guest/admin.vue')
    },
    {
        path: '~/ui/guest/:id',
        component: httpVueLoader('/components/guest/guest.vue'),
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