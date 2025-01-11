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
import { computed, ref } from 'vue'
import { Statuses, StatusId } from '../../shared/models.ts'
import { statusColor } from '@/shared/status-color.ts'

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

// For filtering, show all statuses
// For update, show relevant statuses based on current status
const statusList = computed(() => {
  if (props.isFilter) return Statuses
  else {
    switch (props.currentStatus) {
      case StatusId.Applied:
        return [Statuses[1], Statuses[2], Statuses[3]]
      case StatusId.Rejected:
        return [Statuses[2], Statuses[3]]
      case StatusId.PhoneScreen:
        return [Statuses[1], Statuses[3]]
      case StatusId.Interviewing:
        return [Statuses[4], Statuses[5]]
      case StatusId.NoOffer:
        return [Statuses[5]]
      case StatusId.Offer:
        return [Statuses[6]]
      case StatusId.Accepted:
        return []
    }
    return Statuses
  }
})

// On status change
const onStatusChange = (statusChange: string) => {
  selectedStatus.value = ''
  status.value = statusChange
  emit('statusUpdate', statusChange)
}
</script>
