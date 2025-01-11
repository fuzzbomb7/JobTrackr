<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuth0 } from '@auth0/auth0-vue'

const { isAuthenticated, loginWithRedirect, logout, user } = useAuth0()

const login = () => {
  loginWithRedirect()
}

const logOut = () => {
  logout()
}
</script>

<template>
  <div class="container max-w-screen-md mx-auto">
    <div class="navbar bg-base-100 border-b border-neutral-400">
      <div class="hidden sm:flex sm:flex-1">
        <span class="text-3xl ml-3"
          ><span class="font-light">apply</span><span class="font-bold">Track</span>
          <v-icon name="fa-paper-plane" scale="1.5" class="ml-0.5 mb-1.5"
        /></span>
      </div>
      <div>
        <ul class="menu menu-horizontal gap-1">
          <template v-if="isAuthenticated">
            <li><router-link to="/">My Applications</router-link></li>
            <li><router-link to="/report">Report</router-link></li>
            <li>
              <details>
                <summary>
                  <span class="avatar">
                    <span class="w-6"><img :src="user?.picture" class="rounded-full" /></span>
                  </span>
                  <span class="font-bold mb-1">{{ user?.given_name }}</span>
                </summary>
                <ul class="menu bg-base-200 z-10">
                  <li><a>TOS/Privacy</a></li>
                  <li><a>Contact Us</a></li>
                  <li><a @click.stop="logOut">Logout</a></li>
                </ul>
              </details>
            </li>
          </template>
          <template v-else>
            <li><button class="btn btn-primary" @click="login">Login or Sign Up</button></li>
          </template>
        </ul>
      </div>
    </div>
    <div>
      <router-view />
    </div>
  </div>
</template>
