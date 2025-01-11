<template>
  <template v-if="isAuthenticated">
    <ApplicationList />
  </template>
  <template v-else>
    <div class="bg-yellow-50 border rounded-lg mt-5 mx-4 p-5">
      <p class="text-4xl">
        A <span class="bg-primary rounded-md px-1">simple</span>,
        <span class="bg-secondary rounded-md px-1">free</span> job application tracker
      </p>
      <ul class="list-disc mt-7 ml-5 text-lg">
        <li>Does one thing: tracking job applications. No AI features, resumes, etc.</li>
        <li>Update application status and interview/offer dates as needed.</li>
        <li>Filter applications by company name, date range or application status.</li>
        <li>View your application status report with pie chart.</li>
        <li>Your data remains private. Read our <a href="" class="link">Privacy Policy</a>.</li>
        <li>Sign up and login with Google, Apple or GitHub in just two clicks.</li>
      </ul>
      <div>
        <img src="../assets/applyTrackr-ui.png" alt="applyTrackr" class="mx-auto mt-5 rounded-md" />
      </div>
    </div>
  </template>
</template>

<script setup lang="ts">
import { useAuth0 } from '@auth0/auth0-vue'
import ApplicationList from './application-list/ApplicationList.vue'

const { isAuthenticated } = useAuth0()
</script>
