<template>
  <select
    class="select select-bordered bg-white text-stone-400"
    :class="statusColor(selectedStatus)"
    v-model="selectedStatus"
    @change="onStatusChange(selectedStatus)"
  >
    <option disabled value="" class="text-gray-400">{{ label }}</option>
    <option class="text-black" v-if="isFilter">All Applications</option>
    <option
      v-for="status in statusList"
      :key="status.Value"
      :value="status.Value"
      :class="statusColor(status.Value)"
    >
      {{ status.Text }}
    </option>
  </select>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { Statuses } from '../models.ts'
import { statusColor } from '@/status-color.ts'

const props = defineProps<{
  label: string // Select label
  currentStatus?: string // Current status
  isFilter?: boolean // Is filter select
}>()

const emit = defineEmits<{
  statusUpdate: [status: string]
}>()

// Selected status
const selectedStatus = ref<string>('')
const status = defineModel()

// Remove 'Applied' status for Update Status select
const statusList = props.isFilter ? Statuses : Statuses.slice(1)

// On status change
const onStatusChange = (statusChange: string) => {
  if (statusChange === props.currentStatus) {
    selectedStatus.value = ''
    return
  }
  status.value = statusChange
  emit('statusUpdate', statusChange)
}
</script>
