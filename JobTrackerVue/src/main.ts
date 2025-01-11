import './assets/main.css'
import StatusReport from './components/StatusReport.vue'
import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import { IoOptionsOutline, MdWork, FaPaperPlane } from 'oh-vue-icons/icons'
import { OhVueIcon, addIcons } from 'oh-vue-icons'
import { createAuth0, authGuard } from '@auth0/auth0-vue'
import RootComponent from './components/RootComponent.vue'
import AuthCallback from './components/AuthCallback.vue'
import PrivacyTOS from './components/pages/PrivacyTOS.vue'

addIcons(IoOptionsOutline, MdWork, FaPaperPlane)

const routes = [
  { path: '/', component: RootComponent },
  { path: '/report', component: StatusReport, beforeEnter: authGuard },
  { path: '/authcallback', component: AuthCallback },
  { path: '/privacy', component: PrivacyTOS },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

createApp(App)
  .use(router)
  .use(
    createAuth0({
      domain: import.meta.env.VITE_AUTH0_DOMAIN,
      clientId: import.meta.env.VITE_AUTH0_CLIENT_ID,
      authorizationParams: {
        redirect_uri: import.meta.env.VITE_AUTH0_CALLBACK_URL,
      },
    }),
  )
  .component('v-icon', OhVueIcon)
  .mount('#app')
