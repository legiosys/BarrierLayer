<template>
  <div class="text-center vertical-align-center">
    <div v-if="infoMessage" :class="infoMessage.classes">{{infoMessage.text}}</div>
    <div v-if="guest">
      <p class="alert alert-success" role="alert">Гостевой доступ по адресу:<br><strong>{{ guest?.address }}</strong></p>
      <p class="alert alert-info">Истекает: <br> <strong>{{ expire?.toLocaleString() ?? "" }}</strong></p>
      <button class="btn btn-success" style="align-self: end;" v-on:click.prevent="openBarrier">Открыть шлагбаум</button>
    </div>
  </div>
</template>
<script>
module.exports = {
  name: "guest",
  data: function (){
    return {
      guest: null,
      infoMessage: null,
      expire: Date
    }
  },
  props:['id'],
  computed:{
    errorMessage: function() {
      return {
        classes: "alert alert-danger",
        text: "Гостевой доступ отсутствует"
      }
    }
  },
  created() {
    this.getGuest();
  },
  methods: {
    async getGuest(){
        try{
          const result = await axios.get(`/api/Guest/${this.id}`)
          this.guest = result.data
          this.updateMessage()
        }
        catch (error){
          this.infoMessage = this.errorMessage;
        }
      
    },
    updateMessage(){
      this.expire = this.guest ? new Date(this.guest.expires) : null;
      if(this.expire?.valueOf() < Date.now())
        this.infoMessage = {
          classes: "alert alert-warning",
          text: `Гостевой доступ истек ${this.guest.expires}`
        }
      else {
        this.infoMessage = null;
      }
    },
    async openBarrier(){
      try{
        const result = await axios.post(`/api/Guest/${this.id}`)
        this.guest = result.data
        this.updateMessage()
      }
      catch (error){
        this.infoMessage = this.errorMessage;
      }
    }
  }
}
</script>
<style scoped>
.vertical-align-center {
  height: 100%;
  display: flex;
  align-self: stretch;
  flex-direction: column;
  justify-content: flex-end;
  align-items: center;
  padding-top: 50%;
}
</style>
