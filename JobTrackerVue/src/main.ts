import './assets/main.css'
import ApplicationList from './components/ApplicationList.vue'
import StatusReport from './components/StatusReport.vue'
import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import { IoOptionsOutline } from 'oh-vue-icons/icons'
import { OhVueIcon, addIcons } from 'oh-vue-icons'

addIcons(IoOptionsOutline)

const routes = [
  { path: '/', component: ApplicationList },
  { path: '/report', component: StatusReport },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

createApp(App).use(router).component('v-icon', OhVueIcon).mount('#app')
