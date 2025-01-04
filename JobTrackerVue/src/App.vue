<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <div class="container max-w-screen-lg mx-auto">
    <div class="navbar bg-base-100 border-b border-neutral-400">
      <div class="flex-1">
        <span class="text-3xl ml-3 font-bold">jobTrackr</span>
      </div>
      <div class="flex-none">
        <ul class="menu menu-horizontal gap-1">
          <li><router-link to="/">My Applications</router-link></li>
          <li><router-link to="/report">Report</router-link></li>
        </ul>
      </div>
    </div>
    <div class="">
      <router-view />
    </div>
  </div>
</template>
