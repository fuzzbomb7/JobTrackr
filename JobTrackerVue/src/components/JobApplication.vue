<template>
  <div class="collapse collapse-arrow mb-2 border bg-yellow-50">
    <input type="radio" name="application-list" />
    <div class="collapse-title">
      <span class="font-semibold text-lg">{{ application.jobTitle }}</span>
      <span class="float-right mr-2 mt-1"><ApplicationStatus :status="status" /></span>
      <div class="text-base font-normal text-gray-500">
        {{ application.company }}
      </div>
    </div>
    <div class="collapse-content">
      <div class="divider mt-0"></div>
      <div class="flex">
        <div id="flex-col-1" class="grow">
          <div
            class="flex gap-10 mb-5"
            v-if="
              application.recruiter !== '' || application.email !== '' || application.phone !== ''
            "
          >
            <div class="font-medium" v-if="application.recruiter !== ''">
              <div class="text-sm text-gray-500">Recruiter</div>
              {{ application.recruiter }}
            </div>
            <div v-if="application.email !== ''">
              <div class="text-sm font-medium text-gray-500">Email</div>
              {{ application.email }}
            </div>
            <div v-if="application.phone !== ''">
              <div class="text-sm font-medium text-gray-500">Phone</div>
              {{ application.phone }}
            </div>
          </div>
          <div v-if="application.jobListingUrl !== ''">
            <div class="text-sm font-medium text-gray-500">Job Description URL</div>
            <a :href="application.jobListingUrl" class="link link-info">{{
              application.jobListingUrl
            }}</a>
          </div>
        </div>
        <div id="flex-col-2">
          <div>
            <div>
              <StatusSelect
                @status-update="onStatusChange"
                label="Update Status"
                :current-status="status"
              />
            </div>
            <div class="flex justify-center mt-2" v-show="showProgress">
              <progress class="progress progress-secondary"></progress>
            </div>
          </div>
        </div>
      </div>
      <div class="mt-5">
        <div class="text-sm font-medium text-gray-600">Status History</div>
        <ul class="list-disc list-inside ml-3">
          <li v-for="status in application.statusHistory" :key="status.status">
            <ApplicationStatus :status="status.status" /> on {{ status.date }}
          </li>
        </ul>
      </div>
      <div v-if="application.jobDescription !== ''">
        <div tabindex="0" class="collapse collapse-arrow mt-5 border">
          <div class="collapse-title font-semibold text-info">Job Description</div>
          <div class="collapse-content text-sm">
            {{ application.jobDescription }}
          </div>
        </div>
      </div>
      <div class="flex justify-end mt-4">
        <button class="btn btn-outline btn-error" @click="onDeleteApplication">Delete</button>
      </div>
    </div>
  </div>

  <!-- Congratulations modal -->
  <dialog id="congratulationsModal" ref="congratulationsModal" class="modal">
    <div class="modal-box">
      <form method="dialog">
        <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
      </form>
      <h3 class="text-lg font-bold mb-2">Congratulations!</h3>
      <span class="">Best of luck at your new job!</span>
      <span class="float-right"><img src="../assets/icons8-confetti-64.png" /></span>
    </div>
  </dialog>

  <!-- Status change date modal -->
  <dialog id="statusDateModal" ref="statusDateModal" class="modal">
    <div class="modal-box">
      <form method="dialog">
        <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
      </form>
      <h3 class="text-lg font-bold mb-2"><ApplicationStatus :status="statusChange" /> Date</h3>
      <input
        type="date"
        :max="maxDate"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="statusDate"
      />
      <div class="flex justify-end mt-4">
        <button class="btn btn-primary" @click="onStatusChangeWithDate">Update Status</button>
      </div>
    </div>
  </dialog>
</template>

<script setup lang="ts">
import { computed, ref, useTemplateRef } from 'vue'
import { StatusId, type ApplicationModel } from '../models.ts'
import ApplicationStatus from './ApplicationStatus.vue'
import StatusSelect from './StatusSelect.vue'
import { deleteApplication, patchApplication } from '@/api-service.ts'

// Incoming application model
const application = defineProps<ApplicationModel>()

// Trigger progress bar
const showProgress = ref<boolean>(false)

const emit = defineEmits<{
  updateApplications: [void] // Refresh application list on update
}>()

// Current/max date for status change
const maxDate = new Date().toISOString().split('T')[0]

// Status and status date change
const statusDate = ref<string>(maxDate)
const statusChange = ref<string>('')

// Latest status
const status = computed(
  () => application.statusHistory[application.statusHistory.length - 1].status,
)

// Modal references
const congratulationsModal = useTemplateRef('congratulationsModal')
const statusDateModal = useTemplateRef('statusDateModal')

// Status change
function onStatusChange(newStatus: string) {
  if (newStatus === StatusId.Rejected || newStatus === StatusId.NoOffer) {
    updateStatus(newStatus)
  } else {
    statusChange.value = newStatus
    statusDate.value = maxDate
    statusDateModal.value?.showModal()
  }
}

// Status change with status date selection
function onStatusChangeWithDate() {
  statusDateModal.value?.close()
  updateStatus(statusChange.value, statusDate.value!)
}

// Update application status
function updateStatus(newStatus: string, newStatusDate?: string) {
  showProgress.value = true

  patchApplication(application.id, newStatus, newStatusDate)
    .then(() => {
      showProgress.value = false
      if (newStatus === 'accepted') {
        congratulationsModal.value?.showModal()
      }
      emit('updateApplications')
    })
    .catch(() => {
      alert('There was a problem updating the application status. Please try again.')
      showProgress.value = false
    })
}

// Delete application
function onDeleteApplication() {
  deleteApplication(application.id)
    .then(() => {
      emit('updateApplications')
    })
    .catch(() => {
      alert('There was a problem deleting the application. Please try again.')
    })
}
</script>
