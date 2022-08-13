<template>
  <PageWrapper>
    <template #title>
      {{ $t("newOrder.multiple") }}
    </template>

    <new-order-form />
  </PageWrapper>
</template>

<script>
import NewOrderForm from "../components/NewMultipleOrderForm.vue";
import PageWrapper from "../components/Layout/PageWrapper.vue";

export default {
  components: {
    NewOrderForm,
    PageWrapper,
  },
};
</script>
