<template>
  <div>
    <div class="text-lg font-bold mb-4">Filters</div>
    <div>
      <div class="label"><span class="label-text">Search by Company</span></div>
      <input
        v-model="search"
        @input="onFilterUpdate"
        type="text"
        class="input input-bordered bg-white min-w-64"
      />
    </div>
    <div class="mr-8 mt-2">
      <div class="label"><span class="label-text">Filter by Date Range</span></div>
      <VueDatePicker
        v-model="dateRange"
        @update:model-value="onFilterUpdate"
        @cleared="onFilterUpdate"
        range
        :ui="{ input: 'input input-bordered' }"
        :max-date="new Date()"
        prevent-min-max-navigation
        auto-apply
        :hide-navigation="['time']"
        format="MMM d, yyyy"
        uid="range"
      />
    </div>
    <div class="mt-2">
      <div class="label"><span class="label-text">Filter by Status</span></div>
      <StatusSelect @status-update="onFilterUpdate" v-model="status" label="-" :is-filter="true" />
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FilterUpdatePayload } from '@/shared/models'
import StatusSelect from '../shared/StatusSelect.vue'
import VueDatePicker from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'
import { ref, defineEmits } from 'vue'

const emit = defineEmits<{
  filterUpdate: [filters: FilterUpdatePayload]
}>()

// Filter values
const dateRange = ref<[Date, Date] | null>(null)
const status = ref<string>('')
const search = ref<string>('')

const onFilterUpdate = () => {
  // Emit filter event
  emit('filterUpdate', { dateRange: dateRange.value, status: status.value, search: search.value })
}
</script>

<style>
.dp__input {
  --dp-border-radius: theme(borderRadius.md);
  --dp-font-size: 14px;
}

.dp__theme_light {
  --dp-border-color: theme(colors.gray[300]);
}
</style>
