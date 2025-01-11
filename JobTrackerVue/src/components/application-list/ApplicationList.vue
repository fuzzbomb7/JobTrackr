<template>
  <div class="drawer drawer-end">
    <input type="checkbox" id="filter-drawer" class="drawer-toggle" />
    <div class="drawer-content">
      <div class="flex mx-5 my-5">
        <div class="grow">
          <button class="btn btn-primary" onclick="addApplicationModal.showModal()">
            + Add Application
          </button>
        </div>
        <div>
          <label for="filter-drawer" class="drawer-button btn btn-secondary">
            <div class="badge badge-error" v-if="activeFilterCount > 0">
              {{ activeFilterCount }}
            </div>
            Filter <v-icon name="io-options-outline" />
          </label>
        </div>
      </div>
      <template v-if="applications === undefined || applications.length === 0">
        <LoadingSkeleton />
      </template>
      <template v-else-if="filteredApplications === undefined || filteredApplications.length === 0">
        <div class="text-center text-gray-500">No applications found</div>
      </template>
      <JobApplication
        v-else
        v-for="application in filteredApplications"
        :key="application.id"
        v-bind="{ ...application }"
        @update-applications="getApplications"
      />
    </div>
    <!-- filter drawer -->
    <div class="drawer-side">
      <label for="filter-drawer" aria-label="close sidebar" class="drawer-overlay"></label>
      <div class="menu bg-base-200 text-base-content min-h-full w-80 p-4">
        <ApplicationFilter @filter-update="onFilterUpdate" />
      </div>
    </div>
  </div>

  <!-- Add Application modal -->
  <dialog id="addApplicationModal" class="modal" ref="addApplicationModal">
    <div class="modal-box w-full max-w-max">
      <form method="dialog">
        <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
      </form>
      <h3 class="text-lg font-bold mb-4">Add New Application</h3>
      <AddApplication @add-application="addApplication" />
    </div>
  </dialog>
</template>

<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from 'vue'
import type {
  AddApplicationModel,
  ApplicationModel,
  FilterUpdatePayload,
} from '../../shared/models.ts'
import JobApplication from './JobApplication.vue'
import AddApplication from './AddApplication.vue'
import ApplicationFilter from './ApplicationFilter.vue'
import { getApplications as getApplicationsApi, postApplication } from '../../shared/api-service.ts'
import LoadingSkeleton from '../shared/LoadingSkeleton.vue'

const applications = ref<ApplicationModel[]>([])
const filteredApplications = ref<ApplicationModel[]>()
const activeFilterCount = ref<number>(0)

// Add Application modal reference
const addApplicationModal = useTemplateRef<HTMLDialogElement>('addApplicationModal')

// Get applications on mount
onMounted(() => {
  getApplications()
})

// Handle filter update
const onFilterUpdate = (filters: FilterUpdatePayload) => {
  let filterApplications = applications.value
  activeFilterCount.value = 0

  // Filter by status
  if (filters.status && filters.status !== 'All Applications') {
    filterApplications = filterApplications.filter(
      (application) =>
        application.statusHistory[application.statusHistory.length - 1].status === filters.status,
    )
    activeFilterCount.value++
  }

  // Filter by date range
  if (filters.dateRange) {
    filterApplications = filterApplications.filter(
      (application) =>
        application.statusHistory.find(
          (status) =>
            filters.dateRange &&
            new Date(status.date) >= filters.dateRange[0] &&
            new Date(status.date) <= filters.dateRange[1],
        ) !== undefined,
    )
    activeFilterCount.value++
  }

  // Filter by search
  if (filters.search) {
    filterApplications = filterApplications.filter((application) =>
      application.company.toLowerCase().includes(filters.search.toLowerCase()),
    )
    activeFilterCount.value++
  }

  filteredApplications.value = filterApplications
}

// Get or refresh all applications
const getApplications = () => {
  getApplicationsApi()
    .then((res) => res.json())
    .then((data) => {
      applications.value = data
      filteredApplications.value = data
    })
    .catch(() => alert('There was a problem retrieving the applications. Please try again later.'))
}

// Close modal, add new application, and refetch applications
const addApplication = (application: AddApplicationModel) => {
  addApplicationModal.value?.close()

  postApplication(application)
    .then((res) => res.text())
    .then((data) => {
      if (data === 'false')
        alert('There was a problem adding the application. Please try again later.')
      else getApplications()
      return
    })
    .catch(() => {
      alert('There was a problem adding the application. Please try again later.')
    })
}
</script>
