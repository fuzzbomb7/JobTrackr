<template>
  <div class="bg-yellow-50 border rounded-lg mx-auto p-5 mt-4">
    <div class="grid grid-cols-2 gap-4">
      <div id="report">
        <p>{{ report?.applied ?? 0 }} Jobs <ApplicationStatus :status="StatusId.Applied" /> For</p>
        <p>
          {{ report?.rejected ?? 0 }} Applications
          <ApplicationStatus :status="StatusId.Rejected" />
        </p>
        <p>
          {{ report?.screened ?? 0 }} Applications with Initial
          <ApplicationStatus :status="StatusId.PhoneScreen" />
        </p>
        <p>
          {{ report?.interviewing ?? 0 }} Currently
          <ApplicationStatus :status="StatusId.Interviewing" />
        </p>
        <p>
          {{ report?.noOffer ?? 0 }} Jobs Interviewed with
          <ApplicationStatus :status="StatusId.NoOffer" />
        </p>
        <p>
          {{ report?.offer ?? 0 }} Job(s)
          <ApplicationStatus :status="StatusId.Offer" />
        </p>
        <p>{{ report?.accepted ?? 0 }} Job <ApplicationStatus :status="StatusId.Accepted" />!</p>
      </div>
      <div>
        <Pie
          :data="{
            labels: [
              'Applied',
              'Rejected',
              'Screened',
              'Interviewed',
              'No Offer',
              'Offer',
              'Accepted',
            ],
            datasets: [
              {
                data: [
                  report?.applied ?? 0,
                  report?.rejected ?? 0,
                  report?.screened ?? 0,
                  report?.interviewing ?? 0,
                  report?.noOffer ?? 0,
                  report?.offer ?? 0,
                  report?.accepted ?? 0,
                ],
                backgroundColor: [
                  '#3b82f6',
                  '#ef4444',
                  '#d946ef',
                  '#7c2d12',
                  '#f59e0b',
                  '#8b5cf6',
                  '#10b981',
                ],
              },
            ],
          }"
          :options="{ maintainAspectRatio: false }"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { StatusReportModel } from '@/models'
import { StatusId } from '@/models'
import { onMounted, ref } from 'vue'
import ApplicationStatus from './ApplicationStatus.vue'
import { Pie } from 'vue-chartjs'
import { Chart as ChartJS, registerables } from 'chart.js'
import { getReport } from '@/api-service'

ChartJS.register(...registerables)

const report = ref<StatusReportModel>()
defineExpose({ StatusId })

onMounted(() => {
  getReport()
    .then((response) => response.json())
    .then((data) => {
      report.value = data
    })
    .catch(() => alert('There was a problem retrieving the status report. Please try again.'))
})
</script>
