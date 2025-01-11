<template>
  <div class="grid grid-cols-4 gap-3">
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">
        Job Title <span class="text-red-500">*</span>
      </div>
      <input
        type="text"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.jobTitle"
      />
    </div>
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Company <span class="text-red-500">*</span></div>
      <input
        type="text"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.company"
      />
    </div>
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">
        Application Date <span class="text-red-500">*</span>
      </div>
      <input
        type="date"
        :max="maxDate"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.applicationDate"
      />
    </div>
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Recruiter Name</div>
      <input
        type="text"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.recruiter"
      />
    </div>
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Recruiter Email</div>
      <input
        type="email"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.email"
      />
    </div>
    <div class="sm:col-span-2 col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Phone</div>
      <input
        type="tel"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.phone"
      />
    </div>
    <div class="col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Job Listing URL</div>
      <input
        type="url"
        class="input input-bordered bg-white w-full max-w-full"
        v-model="newApplication.jobListingUrl"
      />
    </div>
    <div class="col-span-4">
      <div class="text-sm text-gray-500 mb-1 ml-3">Job Description</div>
      <textarea
        class="textarea textarea-bordered textarea-md bg-white w-full max-w-full"
        v-model="newApplication.jobDescription"
      ></textarea>
    </div>
    <div class="flex items-center">
      <button class="btn btn-primary" @click="submitNewApplication">Add Application</button>
    </div>
    <div class="sm:col-span-3 col-span-4 sm:ml-5">
      <p v-for="error in errorMessages" :key="error" class="text-red-500">
        {{ error }}
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { AddApplicationModel } from '@/shared/models'
import { computed, ref } from 'vue'
import { useVuelidate } from '@vuelidate/core'
import { required, email, url, helpers } from '@vuelidate/validators'
import { useAuth0 } from '@auth0/auth0-vue'

// Authenticated user ID
const { user } = useAuth0()
const userId = computed(() => user.value?.sub !== undefined ? user.value?.sub : '')

const emit = defineEmits<{
  addApplication: [application: AddApplicationModel]
}>()

// Current date for Application Date input
const maxDate = new Date().toISOString().split('T')[0]

// Form model
const newApplication = ref<AddApplicationModel>({
  jobTitle: '',
  company: '',
  recruiter: '',
  email: '',
  phone: '',
  jobListingUrl: '',
  jobDescription: '',
  applicationDate: maxDate,
  userId: userId.value,
})

// Error messages
const errorMessages = ref<string[]>([])

// Vuelidate validation rules
const phone = helpers.regex(/[\d-().]+/)

const validationRules = {
  email: { email: helpers.withMessage('Please enter a valid email address', email) },
  phone: { phone: helpers.withMessage('Please enter a valid phone number', phone) },
  jobListingUrl: { url: helpers.withMessage('Please enter a valid URL', url) },
  applicationDate: { required },
  jobTitle: { required: helpers.withMessage('Please enter a job title', required) },
  company: { required: helpers.withMessage('Please enter a company name', required) },
}

const v$ = useVuelidate(validationRules, newApplication)

// Validate and submit the form
async function submitNewApplication() {
  // Validate the form
  const validationResult = await v$.value.$validate()
  if (!validationResult) {
    errorMessages.value = v$.value?.$errors.map((error) => error.$message.toString())
    return
  }

  emit('addApplication', newApplication.value)

  // Clear the form
  newApplication.value = {
    jobTitle: '',
    company: '',
    recruiter: '',
    email: '',
    phone: '',
    jobListingUrl: '',
    jobDescription: '',
    applicationDate: maxDate,
    userId: userId.value,
  }

  v$.value.$reset()
  errorMessages.value = []
}
</script>
